using FluentValidation;
using GenApp.WebApi.ActionFilters;
using GenApp.WebApi.Validators;

namespace GenApp.WebApi.Extensions;

public static class Bootstrap
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(configuration);
        services.AddAutoMapper(typeof(Bootstrap).Assembly);
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ResultActionFilter));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddValidatorsFromAssemblyContaining<FileGenSettingsDtoValidator>();

        return services;
    }
}
