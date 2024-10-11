using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace HangFireJob.EventArgs
{
    [Serializable]
    public class TaskArgs
    {

        public TaskArgs() { }


        [Display(Name = "ID")]
        [StringLength(50)]
        [Required]
        public Guid Id { set; get; }



        [Display(Name = "任务ID")]
        [StringLength(50)]
        [Required]
        public string TaskId { set; get; }


        [Display(Name = "任务名称")]
        [StringLength(50)]
        [Required]
        public string Name { set; get; }

        [Display(Name = "操作名称")]
        [StringLength(50)]
        [Required]
        public string ActionName { set; get; }


        //[Required]
        //[Display(Name = "任务状态")]
        //public TaskState State { set; get; }
        //[Display(Name = "错误信息")]
        //[StringLength(50)]

        public string Error { set; get; }
        [Display(Name = "创建人")]
        [StringLength(50)]
        [Required]
        public string Creator { set; get; }
        [Display(Name = "电子邮件")]
        [StringLength(50)]
        [Required]
        public string Email { set; get; }
        //public DateTime CompleteTime { set; get; }

        [Display(Name = "上传文件")]
        [Required]
        public List<string> FileName { set; get; }


        [Display(Name = "上传文件")]
        [Required]
        public List<string> DownFileName { set; get; }


        /// <summary>
        /// 
        /// 上传文件名称（链接）
        /// </summary>
        public List<string> RealFileName { set; get; }


        /// <summary>
        /// 下载文件名称（链接）
        /// </summary>



        /// <summary>
        /// 真实下载文件名称（链接）
        /// </summary>
        public List<string> RealDownFileName { set; get; }

        /// <summary>
        /// 下载文件扩展名
        /// </summary>
        public string Exentsion { set; get; }

        [Display(Name = "执行任务命名空间")]
        [Required]
        public string ServiceName { set; get; }



        public List<CustomCondition> InputConditions { set; get; }

    }

    [Serializable]
    public class CustomCondition
    {
        public string Name { set; get; }
        public string Value
        {

            set; get;

        }
    }
}
