using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Win.Sfs.Shared.DomainBase;

namespace Win.Sfs.Shared.DtoBase
{
    /// <summary>
    /// Entity DTO base
    /// </summary>
    /// <typeparam name="TKey">TKey</typeparam>
    public abstract class EntityDtoBase<TKey> : EntityDto<TKey>, IEnabled, IRemark
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