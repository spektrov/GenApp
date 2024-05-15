using GenApp.DomainServices;
using GenApp.Parsers.Sql;
using GenApp.Parsers.Csharp;
using GenApp.Templates.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace GenApp.IntegrationTests;
internal class TestHost
{
    public TestHost()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; private set; }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSqlParsers();
        services.AddCSharpParsers();
        services.AddDomainServices();
        services.AddTemplateParser();
    }
}
