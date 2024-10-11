using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.Systems.Tokens
{
    public class GetTokenAsync
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;

        public GetTokenAsync(IHttpClientFactory httpClientFactory,IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetBusinessTokenAsync()
        {
            var getToken = string.Empty;
            using (HttpClient clientToken = _httpClientFactory.CreateClient("AuthClientHttpUrl"))
            {
                // 构建请求体
                var request_body = new TokenBodyDto()
                {
                    username = "CronJobUser",//预先内置账号，必须事先存在
                    password = "fD5!pQ8*zX9@Bn2",//预先内置账号密码，必须事先存在
                    client_id = _configuration["GetIndentityToken:Default:ClientId"],
                    grant_type = _configuration["GetIndentityToken:Default:GrantType"],
                    scope = _configuration["GetIndentityToken:Default:Scope"],
                    LoginUrl = "/connect/token",
                    tenant = _configuration["GetIndentityToken:Default:TenantName"]
                };
                var LoginUrl = $"{request_body.LoginUrl}?__tenant={request_body.tenant}";
                var paramPostJwt = new Dictionary<string, string>
                {
                    { "username", request_body.username },
                    { "password", request_body.password },
                    { "client_id", request_body.client_id },
                    { "grant_type", request_body.grant_type },
                    { "scope", request_body.scope }
                };
                var requestToken = new HttpRequestMessage(HttpMethod.Post, LoginUrl);
                requestToken.Headers.Add("Accept", "application/json, text/plain, */*");
                requestToken.Headers.Add("Accept-Language", "zh-Hans");
                requestToken.Headers.Add("Connection", "keep-alive");
                requestToken.Headers.Add("Access-Control-Request-Headers", "authorization");
                requestToken.Headers.Add("Access-Control-Request-Method", "POST");
                requestToken.Headers.Add("Sec-Fetch-Mode", "cors");
                requestToken.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
                requestToken.Content = new FormUrlEncodedContent(paramPostJwt);
                requestToken.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
                var responseToken = await clientToken.SendAsync(requestToken);
                if (responseToken.IsSuccessStatusCode)
                {
                    var body = await responseToken.Content.ReadAsStringAsync();
                    //var tokenInfoBrige = JsonConvert.DeserializeObject<TokenResponse>(body);
                    //getToken = "Bearer " + tokenInfoBrige?.access_token;
                }
                //else
                //{
                //    throw new BusinessException(message: $"访问地址{LoginUrl}，生成token失败！");
                //}
            }

            return getToken;
        }
    }

    public class TokenBodyDto
    {
        public string username { get; set; }
        public string password { get; set; }
        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
        public string LoginUrl { get; set; }
        public string tenant { get; set; }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
    }
}
