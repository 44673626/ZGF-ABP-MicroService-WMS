using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace WMS.Business.EntityFrameworkCore
{
    public class BaseEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
        public static void Configure()
        {
            ConfigureExistingProperties();// 对租户数据库连接表进行扩展
            //其他扩展
        }
        /// <summary>
        /// 对租户数据库连接表进行扩展
        /// </summary>
        private static void ConfigureExistingProperties()
        {
            /* You can change max lengths for properties of the
             * entities defined in the modules used by your application.
             *
             * Example: Change user and role name max lengths
             
               IdentityUserConsts.MaxNameLength = 99;
               IdentityRoleConsts.MaxNameLength = 99;
             
             * Notice: It is not suggested to change property lengths
             * unless you really need it. Go with the standard values wherever possible.
             */
            OneTimeRunner.Run(() =>
            {
                ObjectExtensionManager.Instance.MapEfCoreDbContext<TenantManagementDbContext>(modelBuilder =>
                {
                    modelBuilder.Entity<TenantConnectionString>(b =>
                    {
                        b.ToTable("AbpTenantConnectionStrings");
                        b.Property<string>("DbName").HasMaxLength(300);
                    });
                });
            });
        }

    }
}
