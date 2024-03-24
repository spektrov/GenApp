using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.Sql.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenApp.Parsers.Sql;
public static class Bootstrap
{
    public static IServiceCollection AddSqlParsers(this IServiceCollection services)
    {
        services.AddScoped<ISqlTableParser, SqlTableParser>();

        return services;
    }
}
