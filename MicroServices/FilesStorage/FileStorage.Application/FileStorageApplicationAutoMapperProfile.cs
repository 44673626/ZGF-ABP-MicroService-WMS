using AutoMapper;
using FileStorage.FileManagement.Dto;
using FileStorage.ImportColumnMaps;
using FileStorage.ImportMap;
using FileStorage.Models;

namespace FileStorage
{
    public class FileStorageApplicationAutoMapperProfile : Profile
    {
        public FileStorageApplicationAutoMapperProfile()
        {
            CreateMap<UploadFileInfo, UploadFileInfoDto>();
            CreateMapImportColumnMap();
        }

        private void CreateMapImportColumnMap()

        {
            CreateMap<ImportColumnMap, ImportColumnMapDto>().ReverseMap();
            CreateMap<ImportColumnMap, ImportColumnMapRequestDto>().ReverseMap();
            CreateMap<ImportColumnMap, ImportColumnMapImportDto>().ReverseMap();
            CreateMap<ImportColumnMap, ImportColumnMapExportDto>().ReverseMap();
        }
    }
}
