using AutoMapper;
using HangFireJob.IServices.Dto;
using HangFireJob.Settings;

namespace HangFireJob;

public class HangFireJobApplicationAutoMapperProfile : Profile
{
    public HangFireJobApplicationAutoMapperProfile()
    {
        CreateMap<HttpJobDescriptor, HttpJobDescriptorDto>().ReverseMap();
    }
}
