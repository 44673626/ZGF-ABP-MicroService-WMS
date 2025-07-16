using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.ChunckFiles.Dto
{
    /// <summary>
    /// 文件上传前，获取最大分片的参数
    /// </summary>
    public class MaxChunkDto
    {
        /// <summary>
        /// 前端传的Md5
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Ext { get; set; }
    }
}
