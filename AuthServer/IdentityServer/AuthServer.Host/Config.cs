using AuthServer.Host.Dappers;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

public class Config
{
    public static IEnumerable<IdentityServer4.Models.IdentityResource> GetIdentityResourceResources()
    {
        var customProfile = new IdentityServer4.Models.IdentityResource(
            name: "custom.profile",
            displayName: "Custom profile",
            userClaims: new[] { "companyid" });

        return new List<IdentityServer4.Models.IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                customProfile
            };
    }
}

public class UserProfileService : IProfileService
{
    protected IdentityUserManager UserManager { get; }

    public UserProfileService(IdentityUserManager userManager)
    {
        UserManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        //var companyId = context.Request.Query["company_id"];
        //var companyId = context.Subject.FindFirst(x => x.Type == "company_id")?.Value;
        var user = await UserManager.GetUserAsync(context.Subject);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim("companyid", "1111"),
                new Claim("name",user.Name),
                new Claim("user_name",user.UserName),
                new Claim("email",user.Email),
                new Claim("email_verified",user.EmailConfirmed.ToString()),
                new Claim("phone_number",user.PhoneNumber),
                new Claim("phone_number_verified",user.PhoneNumberConfirmed.ToString())
            };
            // 确保添加用户的角色Claim
            var roles = await UserManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            context.IssuedClaims.AddRange(claims);
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await UserManager.GetUserAsync(context.Subject);
        context.IsActive = user != null;
    }
}

public class CompanyInfo
{
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; }
}

public class UserCompanyIDContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    protected IdentityUserManager UserManager { get; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly CompanyDapperRepository _dapperRepository;

    public UserCompanyIDContributor(
        IdentityUserManager userManager,
        IConfiguration configuration,
        //IUnitOfWorkManager unitOfWorkManager,
        CompanyDapperRepository dapperRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        UserManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _dapperRepository = dapperRepository;
    }
    public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        var userId = identity?.FindUserId();
        if (userId.HasValue)
        {
            var company_id = _httpContextAccessor.HttpContext.Request.Query["company_id"].ToString();
            if (string.IsNullOrEmpty(company_id))
            {
                var user = await UserManager.GetUserAsync(context.ClaimsPrincipal);
                identity.AddClaim(new Claim("username", user.Name));//登录用户名
                if (bool.Parse(_configuration["AppSettings:EnableCompany"]) == true)
                {
                    var companysInfo = await _dapperRepository.GetUserCompanyIds(userId.Value);
                    if (!companysInfo.Any())
                    {
                        //登录用户没有任何公司信息
                        identity.AddClaim(new Claim("companyid", string.Empty));
                        return;
                    }
                    var companyClaims = companysInfo.Select(c => new Dictionary<string, object> { { "CompanyId", c.CompanyId }, { "CompanyName", c.CompanyName } }).ToList();
                    var companiesJson = JsonConvert.SerializeObject(companyClaims);
                    identity.AddClaim(new Claim("companys_info", companiesJson));
                    if (companysInfo.Count() > 1)
                    {
                        //当前登录人属于多个公司,前端通过companys_info绑定后进行选择一个公司登录，重新请求token
                        //并通过传参方式重新传给/connect/token?__tenant=TenantAdmin&company_id=XXXXXXX
                        identity.AddClaim(new Claim("companyid", string.Empty));
                    }
                    else
                    {
                        //当前登录只属于一个公司
                        identity.AddClaim(new Claim("companyid", companysInfo.FirstOrDefault().CompanyId.ToString()));
                    }
                }
                else
                {
                    identity.AddClaim(new Claim("companyid", string.Empty));
                }
            }
            else
            {
                identity.AddClaim(new Claim("companyid", company_id));//动态传参方式
            }
        }
    }
}

public static class CurrentUserExtensions
{
    public static string GetCurrentUserCompanyID(this ICurrentUser currentUser)
    {
        return currentUser.FindClaimValue("companyid");
    }
}

