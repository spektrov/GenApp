using AutoMapper;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.DomainServices.Mappers;
public class TemplatesMapperProfile : Profile
{
    public TemplatesMapperProfile()
    {
        CreateMap<DotnetPropertyConfigurationModel, DotnetPropertyDto>()
            .ForMember(dest => dest.Nullable, src => src.MapFrom(x => x.NotNull ? string.Empty : "?"))
            .ForMember(dest => dest.IsRequired, src => src.MapFrom(x => x.NotNull ? "required " : string.Empty))
            .ForMember(dest => dest.IsCollectionNavigation, src => src.MapFrom(x => x.IsNavigationCollection()));
    }
}
