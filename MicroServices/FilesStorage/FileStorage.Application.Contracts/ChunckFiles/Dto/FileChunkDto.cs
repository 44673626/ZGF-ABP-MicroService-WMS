using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.ChunckFiles.Dto
{
    /// <summary>
    /// 分片上传文件DTO
    /// </summary>
    public class FileChunkDto
    {
        /// <summary>
        /// 当前分片
        /// </summary>
        public int PartNumber { get; set; }

        /// <summary>
        /// 分片总数
        /// </summary>
        public int Chunks { get; set; }
        /// <summary>
        /// 是否开启切片
        /// </summary>
        public bool IsChunkStart { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// md5 key
        /// </summary>
        public string Md5Key { get; set; }

    }

    /// <summary>
    /// 合并分片文件后返回的DTO信息
    /// </summary>
    public class MergeChunkDto
    {
        /// <summary>
        /// 原始上传文件名称
        /// </summary>
        public string OrginFileName { get; set; }
        /// <summary>
        /// 合并切片后生成md5标识的文件名称
        /// </summary>
        public string Md5FileName { get; set; }
        /// <summary>
        /// 生在合并切片后的md5文件路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
