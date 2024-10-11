using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Win.Sfs.Shared.Filter;

namespace Win.Sfs.Shared.DtoBase
{
    /// <summary>
    /// Request DTO base
    /// </summary>
    [Serializable]
    public abstract class BranchRequestDtoBase : RequestDtoBase, IBranch<Guid>
    {
        /// <summary>
        /// 分支ID
        /// </summary>
        [Display(Name = "分支Id")]
        [Required(ErrorMessage = "{0}是必填项")]

        public Guid BranchId { get; set; }
    }

    
}