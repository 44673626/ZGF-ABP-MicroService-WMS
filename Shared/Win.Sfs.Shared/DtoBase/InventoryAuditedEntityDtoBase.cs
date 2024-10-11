// 闻荫智慧工厂管理套件
//  Copyright (c) 闻荫科技 www.ccwin-in.com

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Win.Sfs.Shared.DomainBase;
using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DtoBase
{
    /// <summary>
    /// Full audited entity DTO base
    /// </summary>
    /// <typeparam name="TKey">TKey</typeparam>
    public abstract class InventoryAuditedEntityDtoBase<TKey> : AuditedEntityDto<TKey>,  IRemark
    {
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 看板编号
        /// </summary>
        [Display(Name = "看板编号")]
        public string KanbanNumber {  set; get; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Display(Name = "订单编号")]
        public string PurchaseNumber { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Display(Name = "供应商编码")]
        public string SupplierCode { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        public int OrderType { set; get; }
        /// <summary>
        /// 收货仓库编码
        /// </summary>
        [Display(Name = "收货仓库编码")]
        public string ReceiptWhseCode { set; get; }

        /// <summary>
        /// 收货口编码
        /// </summary>
        [Display(Name = "收货口编码")]
        public string ReceiptPortCode { set; get; }

        /// <summary>
        /// 发货日期
        /// </summary>
        [Display(Name = "发货日期")]
        public DateTime ShipDate {  set; get; }
        /// <summary>
        /// 发货人
        /// </summary>
        [Display(Name = "发货人")]
        public string ShipUser {  set; get; }


        /// <summary>
        /// 单据流水号
        /// </summary>
        [Display(Name = "单据流水号")]
        public string DocumentNumber { get; set; }


        /// <summary>
        /// 单据类型
        /// </summary>
        [Display(Name = "单据类型")]
        [Required(ErrorMessage = "{0}是必填项")]
        public EnumDocumentType DocumentType { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        [Required(ErrorMessage = "{0}是必填项")]
        public EnumDocumentStatus DocumentStatus { get; set; }

        /// <summary>
        /// 历史单据信息
        /// </summary>
        [Display(Name = "历史单据信息")]
        public List<UpstreamDocument> UpstreamDocumentDataBases { get; set; }


    }
}