using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.Systems.MessageManagement
{
    public class OnlineUsers
    {
        /// <summary>
        /// 客户端连接Id
        /// </summary>
        public string ConnnectionId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 在线时长
        /// </summary>
        public double OnlineTime
        {
            get
            {
                var ts = DateTime.Now - LoginTime;
                return Math.Round(ts.TotalMinutes, 2);
            }
        }
    }
}
