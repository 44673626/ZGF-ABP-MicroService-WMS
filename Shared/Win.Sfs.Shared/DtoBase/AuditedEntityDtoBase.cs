// 闻荫智慧工厂管理套件
//  Copyright (c) 闻荫科技 www.ccwin-in.com

using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Win.Sfs.Shared.DomainBase;

namespace Win.Sfs.Shared.DtoBase
{
    /// <summary>
    /// Full audited entity DTO base
    /// </summary>
    /// <typeparam name="TKey">TKey</typeparam>
    public abstract class AuditedEntityDtoBase<TKey> : AuditedEntityDto<TKey>, IEnabled, IRemark
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        [Display(Name = "是否可用")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}