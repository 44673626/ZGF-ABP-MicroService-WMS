using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileStorage.ImportMap
{
    public class ImportColumnMap : FullAuditedAggregateRoot<Guid>
    {
        public ImportColumnMap() { }
        public ImportColumnMap(Guid id, string projectName, string oldColumnName, string newColumnName, bool ischeck) : base(id)
        {
            ProjectName = projectName;
            OldColumnName = oldColumnName;
            NewColumnName = newColumnName;
            IsCheck = ischeck;
        }

        public string ProjectName { set; get; }
        public string OldColumnName { set; get; }
        public string NewColumnName { set; get; }
        public bool IsCheck { set; get; }

        public void Update(string newcolumn)
        {
            NewColumnName = newcolumn;

        }
    }
}
