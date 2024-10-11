using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles.ExportReports
{
    /// <summary>
    /// 输出文件导入时的校验信息
    /// </summary>
    public class ErrorExportDto : EntityDto
    {

        public ErrorExportDto()
        {
        }

        public ErrorExportDto(string version, string customCode, string type, string model, string itemCode, string itemDesc, string message, string remark)
        {
            Version = version;
            CustomCode = customCode;
            Type = type;
            Model = model;
            ItemCode = itemCode;
            ItemDesc = itemDesc;
            Message = message;
            Remark = remark;
        }

        /// <summary>
        /// 版本
        /// </summary>
        [Display(Name = "版本")]
        public string Version { set; get; }


        /// <summary>
        /// 客户代码
        /// </summary>

        [Display(Name = "客户代码")]
        public string CustomCode { get; set; }

        /// <summary>
        /// 影响类型
        /// </summary>

        [Display(Name = "影响类型")]
        public string Type { get; set; }

        /// <summary>
        /// 问题模块
        /// </summary>

        [Display(Name = "问题模块")]
        public string Model { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        [Display(Name = "物料号")]

        public string ItemCode { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        [Display(Name = "物料描述")]

        public string ItemDesc { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>

        [Display(Name = "提醒信息")]
        public string Message { get; set; }

        /// <summary>
        /// 金额差异
        /// </summary>

        [Display(Name = "备注")]
        public string Remark { get; set; }


    }
}
