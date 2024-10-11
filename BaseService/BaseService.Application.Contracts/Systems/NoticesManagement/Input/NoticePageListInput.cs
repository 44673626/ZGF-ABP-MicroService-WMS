using BaseService.Systems.MessageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.Systems.NoticesManagement.Input
{
    public class NoticePageListInput : BasePageInput
    {
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

        /// <summary>
        /// 公告状态 (0正常 1关闭)
        /// </summary>
        public int? Status { get; set; }


        public string Remark { get; set; }
    }
}
