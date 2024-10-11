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

namespace BaseService.Tokens
{
    /// <summary>
    /// 获取Jwt的Token并模拟用户登录，从而实现权限的认证
    /// </summary>
    internal class JwtTokenHttpContext
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly HttpContext _httpContext;

        protected IDictionary<string, string?> _header;

        public JwtTokenHttpContext(IServiceProvider serviceProvider)
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
        /// 获取token请求头
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public JwtTokenHttpContext SetTokenHeader(IDictionary<string, string?> header)
        {
            _header = header;
            return this;
        }

        public async void JwtToken()
        {
            if (_header == null) throw new BusinessException("Header is null");

            if (_header.ContainsKey("Authorization")
                && !String.IsNullOrEmpty(_header["Authorization"]))
            {
                var tokenString = _header["Authorization"];
                // 注入Token，使Job能获取ICurrentUser，并使EF.core能获取到当前租户等信息
                SignInAuthorization(tokenString).SignInUser(tokenString);
                // 模拟身份验证
                var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var authService = _serviceProvider.GetRequiredService<IAuthenticationService>();
                var result = authService.AuthenticateAsync(httpContext, "Bearer").Result;
                if (!result.Succeeded)
                {
                    throw new BusinessException("身份验证失败.");
                }
            }

            if (_header.ContainsKey("X-Company-Id")
                && !String.IsNullOrEmpty(_header["X-Company-Id"]))
            {
                var companyId = _header["X-Company-Id"];

                // 注入Token，使Job能获取ICurrentUser，并使EF.core能获取到当前租户等信息
                InUserCompanyId(companyId);
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
                //模拟token验证
                claims.Add(new Claim(ClaimTypes.NameIdentifier, sPrincipal.Claims.FirstOrDefault(p => p.Type == "sub")?.Value));
                claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", sPrincipal.Claims.FirstOrDefault(p => p.Type == "idp")?.Value));
                claims.Add(new Claim(ClaimTypes.Email, sPrincipal.Claims.FirstOrDefault(p => p.Type == "email")?.Value));
                claims.Add(new Claim("http://schemas.microsoft.com/claims/authnmethodsreferences", sPrincipal.Claims.FirstOrDefault(p => p.Type == "amr")?.Value));
                sPrincipal.Claims.Where(p => p.Type == "role").Select(p => p.Value).ToList().ForEach(value =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, value));
                });
                // 用户登录成功，添加认证方法名称声明,以保证identity.IsAuthenticated 返回 true，即表示用户身份验证成功
                claims.Add(new Claim(ClaimTypes.AuthenticationMethod, "Password"));
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Identity.AppLication"));

            }
            catch (SecurityTokenValidationException ex)
            {
                // 处理 token 验证失败的情况
                // 可以根据具体需求进行错误处理或抛出异常
                throw new Exception("token格式不正确，请检查！");
            }
            return claimsPrincipal;

        }

        /// <summary>
        /// 注入Token
        /// </summary>
        /// <param name="authorizationString"></param>
        private JwtTokenHttpContext SignInAuthorization(string authorizationString)
        {
            _httpContext.Request.Headers["Authorization"] = authorizationString;

            return this;
        }

        private JwtTokenHttpContext SignInUser(string authorizationString)
        {
            var context = _httpContext;

            // 从token读取ClaimsPrincipal
            var principal = GetClaimsPrincipal(authorizationString);
            //在 ASP.NET Core 应用程序中，当用户成功登录，身份验证中间件将设置 HttpContext.User 为一个包含 ClaimsIdentity 的 ClaimsPrincipal。
            context.User = principal;
            context.RequestServices = _serviceProvider;

            return this;
        }

        private JwtTokenHttpContext InUserCompanyId(string companyId)
        {
            _httpContext.Request.Headers["X-Company-Id"] = companyId;

            return this;
        }

    }
}
