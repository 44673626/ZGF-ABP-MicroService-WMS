using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseEntity.UnitConversions.Dtos;
using WMS.BaseService.BusinessEntity;

namespace WMS.BaseService.BaseEntity.UnitConversions.Profiles
{
    /// <summary>
    /// 自动映射
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUnitConversionDto, UnitConversion>().ReverseMap();
            CreateMap<ModifyUnitConversionDto, UnitConversion>().ReverseMap();
            CreateMap<UnitConversion, UnitConversionDto>().ReverseMap();
        }

    }
}
