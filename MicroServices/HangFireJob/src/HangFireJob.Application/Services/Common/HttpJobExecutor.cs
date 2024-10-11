using Hangfire;
using HangFireJob.IServices.Dto;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace HangFireJob.Services.Common
{
    public class HttpJobExecutor
    {
        // [HttpJobFilter]
        public static void DoRequest(HttpJobDescriptorDto jobDestriptor)
        {
            var client = new RestClient(jobDestriptor.HttpUrl);
            var httpMethod = (object)Method.Post;
            if (!Enum.TryParse(typeof(Method), jobDestriptor.HttpMethod, out httpMethod))
                throw new Exception($"不支持的HTTP动词：{jobDestriptor.HttpMethod}");
            var request = new RestRequest(string.Empty, (Method)httpMethod);
            if (jobDestriptor.HttpMethod == "Get")
            {
                request.AddHeader("Content-Type", "application/json");
                request.Timeout = 1000 * 60 * 5; // 限制时间 5分钟
                foreach (var item in jobDestriptor.GetParams)
                {
                    //key参数值不为空，才动态添加参数，底层Key要求不能为空，否则报错，考虑还有不传参的接口，此处只做过滤
                    if (!string.IsNullOrEmpty(item.Key))
                    {
                        request.AddParameter(item.Key, item.Value.ToString());
                    }
                }
            }
            else
            {
                var json = string.Empty;
                if (jobDestriptor.JobParameter != null)
                {
                    if (jobDestriptor.JobParameter is string _dataStr)
                    {
                        json = _dataStr;
                    }
                    else
                    {
                        json = JsonConvert.SerializeObject(jobDestriptor.JobParameter);
                    }
                }
                request.AddParameter("application/json", json, ParameterType.RequestBody);
            }
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"调用接口{jobDestriptor.HttpUrl}失败，接口返回：{response.Content}");
        }




        //public static void DoRequest(HttpJobDescriptor jobDescriptor)
        //{
        //    var client = new RestClient(jobDescriptor.HttpUrl);
        //    if (!Enum.TryParse(jobDescriptor.HttpMethod.ToUpper(), out Method httpMethod))
        //        throw new Exception($"不支持的HTTP动词：{jobDescriptor.HttpMethod}");
        //    var request = new RestRequest("",httpMethod);
        //    if (jobDescriptor.JobParameter != null)
        //    {
        //        var json = JsonConvert.SerializeObject(jobDescriptor.JobParameter);
        //        request.AddParameter("application/json", json, ParameterType.RequestBody);
        //    }
        //    var response = client.Execute(request);
        //    if (response.StatusCode != HttpStatusCode.OK)
        //        throw new Exception($"调用接口{jobDescriptor.HttpUrl}失败，接口返回：{response.Content}");
        //ApiInput apiInput = new ApiInput();
        //apiInput.ApiUrl = ""; 
        //    apiInput.PostParams = JsonConvert.SerializeObject(input);
        //    var content = HxApiHelper.PostApi(apiInput);
        //}


    }


}
