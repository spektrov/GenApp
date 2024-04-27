using GenApp.WebApi.Models;

namespace GenApp.WebApi.Extensions;

public static class AddCorsExtension
{
    public static void AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApiConstants.CorsPolicy, builder => builder
                .WithOrigins(configuration["FrontendUrl"] ?? string.Empty)
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }
}
