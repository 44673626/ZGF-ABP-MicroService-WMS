using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.Systems.NoticesManagement.Input
{
    public class UpdateNoticeInput : NoticeDtoBase
    {
        public Guid Id { get; set; }
    }
}
