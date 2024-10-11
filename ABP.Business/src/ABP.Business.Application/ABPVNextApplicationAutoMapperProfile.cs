using AutoMapper;
using ABP.Business.Samples;
using ABP.Business.Samples.Boms.Dto;
using ABP.Business.Samples.DataDictionarys;
using Volo.Abp.AutoMapper;

namespace ABP.Business;

public class ABPVNextApplicationAutoMapperProfile : Profile
{
    public ABPVNextApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMapBom();
        CreateMapDicion();
    }

    private void CreateMapBom()
    {
        CreateMap<Bom, BomDto>().ReverseMap();
        CreateMap<ImportBomDto, Bom>().ReverseMap();
        CreateMap<Bom, BomExportDto>().ReverseMap();
    }
    /// <summary>
    /// 测试用例，应用缓存
    /// </summary>
    private void CreateMapDicion()
    {
        CreateMap<DataDictionary, DictionaryDto>();

        CreateMap<DictionaryCreateDto, DataDictionary>()
               .IgnoreAuditedObjectProperties()
               .Ignore(x => x.ConcurrencyStamp)
               .Ignore(x => x.Id);

        CreateMap<DictionaryUpdateDto, DataDictionary>()
           .IgnoreAuditedObjectProperties()
           .Ignore(x => x.TenantId)
           .Ignore(x => x.Id);

    }
}
