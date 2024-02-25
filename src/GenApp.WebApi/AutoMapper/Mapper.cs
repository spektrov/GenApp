using AutoMapper;
using GenApp.Domain.Models;
using GenApp.WebApi.Models;

namespace GenApp.WebApi.AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<GenSettingsDto, GenSettingsModel>()
            .ForMember(dest => dest.SqlTableScript, src => src.MapFrom(x => x.SqlScript));
    }
}
