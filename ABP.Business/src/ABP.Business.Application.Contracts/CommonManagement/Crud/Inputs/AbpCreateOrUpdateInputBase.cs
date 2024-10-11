using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace ABP.Business.CommonManagement.Crud.Inputs
{
    /// <summary>
    /// 创建或更新DTO基础服务
    /// </summary>
    public abstract class AbpCreateOrUpdateInputBase : EntityDto, IHasExtraProperties, IMultiTenant
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        [DefaultValue(null)]
        public Guid? TenantId { get; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        [DefaultValue("{}")]
        public ExtraPropertyDictionary ExtraProperties { get; }
    }
}
