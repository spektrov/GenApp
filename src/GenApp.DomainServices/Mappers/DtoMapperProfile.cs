using AutoMapper;
using GenApp.Domain.Models;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Mappers;
public class DtoMapperProfile : Profile
{
    public DtoMapperProfile()
    {
        CreateMap<GenSettingsModel, ExtendedGenSettingsModel>()
            .ForMember(dest => dest.TableConfiguration, src => src.MapFrom<SqlTableScriptResolver>())
            .ForMember(dest => dest.DotnetSdkVersion, src => src.MapFrom<DotnetSdkVersionResolver>());

        CreateMap<SqlTableConfigurationModel, DotnetEntityConfigurationModel>()
            .ConvertUsing<DotnetEntityConfigurationConverter>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        CreateMap<DotnetPropertyConfigurationModel, DotnetPropertyModel>()
            .ForMember(dest => dest.Nullable, src => src.MapFrom(x => x.NotNull ? string.Empty : "?"));
    }
}
