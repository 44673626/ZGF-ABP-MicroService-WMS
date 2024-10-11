using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace Win.Sfs.FileStorage.UploadFile
{
    /// <summary>
    /// 文件类型的Blob容器
    /// </summary>
    [BlobContainerName("win-file-container")]
    public class BlobCommonFileContainer
    {
        protected BlobCommonFileContainer()
        { }

    }
}
