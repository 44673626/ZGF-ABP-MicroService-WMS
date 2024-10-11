using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Win.Sfs.FileStorage.UploadFile
{
    public class BlobData : Entity<Guid>
    {
        //需要保留无参构造函数
        protected BlobData()
        {
        }
        public byte[] Content { get; set; }

        public string Name { get; set; }

        public BlobData(Guid id,
            string name,
             byte[] content)
        : base(id)
        {
            Name = name;
            Content = content;
        }
    }
}
