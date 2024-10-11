using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles.ExportReports
{
    public class ImportProjectAttribute : Attribute
    {
        public ImportProjectAttribute()
        {
        }
        public string Name { set; get; }
    }
}
