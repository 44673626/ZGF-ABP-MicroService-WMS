using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.Business.Logs
{
    /// <summary>
    /// 提供前端的提示消息模型
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string? code { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; } = 200;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; } = false;

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; } = "";

        /// <summary>
        /// 详细消息
        /// </summary>
        public string msgDev { get; set; } = "";

        /// <summary>
        /// 请求
        /// </summary>
        public object? response { get; set; } = null;

        /// <summary>
        /// 字典
        /// </summary>
        public IDictionary ext { get; set; }
    }
}
