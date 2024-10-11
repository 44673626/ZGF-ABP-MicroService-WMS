using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FileStorage.ImportColumnMaps
{

    public class ImportColumnMapDto : EntityDto<Guid>

    {

        public string ProjectName { get; set; }
        public string OldColumnName { get; set; }
        public string NewColumnName { get; set; }
        public bool IsCheck { set; get; }

    }



    public class ImportColumnMapExportDto

    {

        public string ProjectName { get; set; }
        public string OldColumnName { get; set; }
        public string NewColumnName { get; set; }
        public bool IsCheck { set; get; }

    }



    public class ImportColumnMapImportDto

    {

        public string ProjectName { get; set; }
        public string OldColumnName { get; set; }
        public string NewColumnName { get; set; }
        public bool IsCheck { set; get; }

    }



    public class ImportColumnMapRequestDto : PagedAndSortedResultRequestDto

    {

        public string ProjectName { get; set; }
        public string OldColumnName { get; set; }
        public string NewColumnName { get; set; }
        public bool IsCheck { set; get; }

       // public virtual List<FilterCondition> Filters { get; set; } = new List<FilterCondition>();
    }
}
