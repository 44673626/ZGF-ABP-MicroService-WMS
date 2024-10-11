using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.Systems.NoticesManagement
{
    public class NoticeDtoBase
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 公告类型 (1通知 2公告)
        /// </summary>
        public int NoticeType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string NoticeContent { get; set; }

        public string CreateBy { get; set; }
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 公告状态 (0正常 1关闭)
        /// </summary>
        public int Status { get; set; }


        public string Remark { get; set; }
    }
}
