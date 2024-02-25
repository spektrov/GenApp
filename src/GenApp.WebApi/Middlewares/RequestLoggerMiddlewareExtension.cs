using System.Diagnostics;

namespace GenApp.WebApi.Middlewares;

public class RequestLoggerMiddlewareExtension
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggerMiddlewareExtension> _logger;

    public RequestLoggerMiddlewareExtension(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestLoggerMiddlewareExtension>();
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        await _next(context);

        _logger.LogInformation(
            "Requested endpoint: {Method} {Endpoint}\nStatus code: {StatusCode}\nExecution time: {Time}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode.ToString(),
            stopwatch.Elapsed.ToString());
    }
}
