using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Net;
using GenApp.WebApi.Models;

namespace GenApp.WebApi.Middlewares;

public static class ExceptionMiddlewareExtension
{
    public static void UseErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError("Something went wrong: {ContextFeatureError}", contextFeature.Error);

                    await context.Response.WriteAsJsonAsync(
                        new ErrorDetails(context.Response.StatusCode, "Internal Server Error."));
                }
            });
        });
    }
}
