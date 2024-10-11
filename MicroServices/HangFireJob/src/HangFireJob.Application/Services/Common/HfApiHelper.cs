using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.Services.Common
{
    public class HfApiHelper
    {
        /// <summary>
        /// Post方式调用的接口方法.
        /// </summary>
        /// <param name="input">入参.</param>
        /// <returns>接口返回的结果string.</returns>
        public static string PostApi(ApiInput input)
        {
            string contentType = "application/json";

            // 调用接口
            var client = new RestClient(input.ApiUrl);
            var request = new RestRequest(string.Empty, Method.Post);
            request.Timeout = 1000 * 60 * 5; // 限制时间 5分钟
            request.AddParameter(contentType, input.PostParams, ParameterType.RequestBody);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"调用接口{input.ApiUrl}失败，接口返回：{content}");
            return content;
        }

        /// <summary>
        /// Get方式调用的接口方法.
        /// </summary>
        /// <param name="input">入参.</param>
        /// <returns>接口返回的结果string.</returns>
        public static string GetApi(ApiInput input)
        {
            // 调用接口
            var client = new RestClient(input.ApiUrl);
            var request = new RestRequest(string.Empty, Method.Get);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = 1000 * 60 * 5; // 限制时间 5分钟
            if (input.GetParams != null)
            {
                foreach (var item in input.GetParams)
                {
                    request.AddParameter(item.Key, item.Value.ToString());
                }
            }
            var response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"调用接口{input.ApiUrl}失败，接口返回：{content}");
            return content;
        }

        ///// <summary>
        ///// Get方式调用的接口方法无参数.
        ///// </summary>
        ///// <param name="input">入参.</param>
        ///// <returns>接口返回的结果string.</returns>

        //public static string GetApiNoParam(ApiInput input)
        //{
        //    var client = new RestClient(input.ApiUrl);
        //    var request = new RestRequest();//get提交
        //    var response = client.Execute(request);
        //    if (response.StatusCode != HttpStatusCode.OK)
        //        throw new Exception($"调用接口{input.ApiUrl}失败，接口返回：{content}");
        //    var content = response.Content;
        //    return content;
        //}


    }
}
