using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Interfaces;
using GenApp.Parsers.CSharp.Mappers;
using GenApp.Parsers.CSharp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenApp.Parsers.Csharp;
public static class Bootstrap
{
    public static IServiceCollection AddCSharpParsers(this IServiceCollection services)
    {
        services.AddScoped<ICaseTransformer, CaseTransformer>();
        services.AddScoped<IDotnetEntityFactory, DotnetEntityFactory>();
        services.AddScoped<IDotnetRelationMapper, DotnetRelationMapper>();

        return services;
    }
}