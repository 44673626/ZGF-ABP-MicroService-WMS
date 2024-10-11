using ABP.Business.CommonManagement.Tenants.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ABP.Business.CommonManagement.Tenants
{
    public interface ITenantManagerAppService : IApplicationService
    {
        Task CreateTenantDataBaseWithSeedData(bool isTenantConnExist = false);

        Task CreateTenantDataBase(bool isTenantConnExist = false);

        Task TenantUploadSqlScriptImport([FromForm] IFormFileCollection files, Guid? tenantId);

        Task<int> CreateAbpTenantConnectionStrings(TenantConnectionStringsDto dto);

        //Task DownloadBigFile(string fileId, int index);
    }
}
