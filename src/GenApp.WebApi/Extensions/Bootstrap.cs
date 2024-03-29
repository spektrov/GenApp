﻿using GenApp.WebApi.ActionFilters;

namespace GenApp.WebApi.Extensions;

public static class Bootstrap
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Bootstrap).Assembly);
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ResultActionFilter));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
