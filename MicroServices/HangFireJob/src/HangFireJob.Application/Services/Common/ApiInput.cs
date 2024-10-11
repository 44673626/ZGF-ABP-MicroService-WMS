using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.Services.Common
{
    /// <summary>
    /// 调用api接口的入参.
    /// </summary>
    public class ApiInput
    {
        /// <summary>
        /// api接口地址.
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// api post方式的入参参数 Json.
        /// </summary>
        public string PostParams { get; set; }

        /// <summary>
        /// api Get方式的入参参数
        /// </summary>
        public List<GetParamModel> GetParams { get; set; }


        /// <summary>
        /// Get参数的结构
        /// </summary>
        public class GetParamModel
        {
            /// <summary>
            /// key.
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// value值.
            /// </summary>
            public string Value { get; set; }
        }
    }
}
