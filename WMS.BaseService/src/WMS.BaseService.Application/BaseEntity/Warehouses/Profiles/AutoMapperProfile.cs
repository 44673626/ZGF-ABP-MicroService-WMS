using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseContracts;
using WMS.BaseService.BaseEntity.Warehouses.Dtos;
using WMS.BaseService.BusinessEntity;

namespace WMS.BaseService.BaseEntity.Warehouses.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateWarehouseDto, Warehouse>().ReverseMap();
            CreateMap<ModifyWarehouseDto, Warehouse>().ReverseMap();
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(r => r.Status, it => it.MapFrom(c => c.Status.GetHasCodeStr()))
                .ForMember(r => r.StatusName, it => it.MapFrom(c => c.Status.GetDisplayName()))
                .ForMember(r => r.WarehouseType, it => it.MapFrom(c => c.WarehouseType.GetHasCodeStr()))
                .ForMember(r => r.WarehouseTypeName, it => it.MapFrom(c => c.WarehouseType.GetDisplayName()))
                .ReverseMap();
        }
    }
}
