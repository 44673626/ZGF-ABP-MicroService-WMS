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
    public abstract class RequestDtoBase : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 筛选条件
        /// </summary>
        [Display(Name = "筛选条件")]
        public virtual List<FilterCondition> Filters { get; set; } = new List<FilterCondition>();
    }

    
}