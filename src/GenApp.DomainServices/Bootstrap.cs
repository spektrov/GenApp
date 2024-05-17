using GenApp.Domain.Interfaces;
using GenApp.DomainServices.Mappers;
using GenApp.DomainServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenApp.DomainServices;
public static class Bootstrap
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Bootstrap).Assembly);
        AddCommands(services);
        services.AddScoped<ISolutionGenService, SolutionGenService>();
        services.AddScoped<IFileGenService, FileGenService>();
        services.AddScoped<IArchiveGenService, ArchiveGenService>();
        services.AddScoped<IApplicationDataMapper, ApplicationDataModelMapper>();
        services.AddScoped<IConnectionDetailsProvider, ConnectionDetailsProvider>();

        return services;
    }

    private static void AddCommands(IServiceCollection services)
    {
        var commandImplementationTypes = typeof(Bootstrap).Assembly.GetTypes()
            .Where(type => typeof(IGenCommand).IsAssignableFrom(type) && !type.IsInterface);

        foreach (var commandImplementationType in commandImplementationTypes)
        {
            services.AddScoped(typeof(IGenCommand), commandImplementationType);
        }
    }
}
