using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace WMS.BaseService.CAPToken
{
    /// <summary>
    /// 模拟HttpContext
    /// CAP的方法，因未走http协议，故，没有HttpContext
    /// </summary>
    public class CapHttpContext
    {
        IServiceProvider _serviceProvider;

        HttpContext _httpContext;

        IDictionary<string, string?> _header;

        public CapHttpContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();

            if (httpContextAccessor.HttpContext == null)
            {
                httpContextAccessor.HttpContext = new DefaultHttpContext();

                _httpContext = httpContextAccessor.HttpContext;
            }

        }

        /// <summary>
        /// 设置CAP 请求头
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public CapHttpContext SetCAPHeader(IDictionary<string, string?> header)
        {
            _header = header;
            return this;
        }

        public void MockToken()
        {
            if (_header == null) throw new BusinessException("Header is null");

            if (_header.ContainsKey("Authorization")
                && !String.IsNullOrEmpty(_header["Authorization"]))
            {
                var tokenString = _header["Authorization"];

                // 注入Token，获取ICurrentUser，并使EF.core能获取到当前租户等信息
                InjectAuthorization(tokenString).PrincipalUser(tokenString);

                // 模拟身份验证
                var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

                var authService = _serviceProvider.GetRequiredService<IAuthenticationService>();
                var result = authService.AuthenticateAsync(httpContext, "Bearer").Result;

                if (!result.Succeeded)
                {
                    throw new BusinessException("CAP模拟Http协议，身份验证失败.");
                }
            }

            if (_header.ContainsKey("X-Company-Id")
                && !String.IsNullOrEmpty(_header["X-Company-Id"]))
            {
                var companyId = _header["X-Company-Id"];

                // 注入Token，能获取到当前租户等信息
                InjectCompanyId(companyId);
            }
        }

        private ClaimsPrincipal GetClaimsPrincipal(string tokenString)
        {
            if (tokenString == null) throw new ArgumentNullException(nameof(tokenString));

            if (tokenString.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                tokenString = tokenString.Substring("Bearer ".Length).Trim();
            }


            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal claimsPrincipal;
            try
            {
                // 验证并解析 token，获取 ClaimsPrincipal
                var sPrincipal = jwtSecurityTokenHandler.ReadJwtToken(tokenString);

                var claims = new List<Claim>();
                claims.AddRange(sPrincipal.Claims);

                /*
                    一些ABP自定义的Claim转换成标准的Claim,否则ICurrentUser无法获取到这些Claim
                 */
                claims.Add(new Claim(ClaimTypes.NameIdentifier, sPrincipal.Claims.FirstOrDefault(p => p.Type == "sub")?.Value));
                claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", sPrincipal.Claims.FirstOrDefault(p => p.Type == "idp")?.Value));

                claims.Add(new Claim(ClaimTypes.Email, sPrincipal.Claims.FirstOrDefault(p => p.Type == "email")?.Value));
                claims.Add(new Claim("http://schemas.microsoft.com/claims/authnmethodsreferences", sPrincipal.Claims.FirstOrDefault(p => p.Type == "amr")?.Value));

                sPrincipal.Claims.Where(p => p.Type == "role").Select(p => p.Value).ToList().ForEach(value =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, value));
                });
                claims.Add(new Claim(ClaimTypes.AuthenticationMethod, "Password"));
                // 创建 ClaimsPrincipal 实例，并使用身份验证类型和 ClaimsIdentity
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Identity.Application"));//LocalAuthentication

            }
            catch (SecurityTokenValidationException ex)
            {
                // 处理 token 验证失败的情况
                // 可以根据具体需求进行错误处理或抛出异常
                throw new Exception("读取token失败！");
            }
            return claimsPrincipal;
            // 现在，claimsPrincipal 中包含了解析后的声明信息

        }

        /// <summary>
        /// 注入Token
        /// </summary>
        /// <param name="authorizationString"></param>
        private CapHttpContext InjectAuthorization(string authorizationString)
        {
            _httpContext.Request.Headers["Authorization"] = authorizationString;

            return this;
        }

        private CapHttpContext PrincipalUser(string authorizationString)
        {
            var context = _httpContext; 
            // 从token读取ClaimsPrincipal
            var principal = GetClaimsPrincipal(authorizationString);
            //将 ClaimsPrincipal 设置为当前用户
            context.User = principal;
            context.RequestServices = _serviceProvider;
            return this;
        }

        private CapHttpContext InjectCompanyId(string companyId)
        {
            _httpContext.Request.Headers["X-Company-Id"] = companyId;

            return this;
        }

    }
}
