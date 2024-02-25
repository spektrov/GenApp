namespace GenApp.WebApi.Middlewares;

public static class UseRequestLoggerExtension
{
    public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseMiddleware<RequestLoggerMiddlewareExtension>();
        return app;
    }
}
