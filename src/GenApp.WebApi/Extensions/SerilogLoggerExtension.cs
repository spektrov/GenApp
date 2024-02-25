using Serilog;

namespace GenApp.WebApi.Extensions;

public static class SerilogLoggerExtension
{
    public static void AddSerilogLogging(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        loggingBuilder.AddSerilog(logger);
    }
}
