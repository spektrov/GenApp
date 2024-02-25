using GenApp.Templates.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace GenApp.Templates.Parser;

public static class Bootstrap
{
    public static IServiceCollection AddTemplateParser(this IServiceCollection services)
    {
        services.AddScoped<ITemplateParser, TemplateParser>();

        return services;
    }
}