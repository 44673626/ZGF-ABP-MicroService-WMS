using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace ABP.Business.CommonManagement.UploadBlobFiles
{
    /// <summary>
    /// 用于存储模板文件（独立于上传文件容器）
    /// </summary>
    [BlobContainerName("abp-templateFile-container")]
    public class BlobTemplateFileContainer
    {
        protected BlobTemplateFileContainer()
        { }

    }
}
