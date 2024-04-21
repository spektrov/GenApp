using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;
using System.Net.Mime;

namespace GenApp.WebApi.Middlewares;

public static class ExceptionMiddlewareExtension
{
    public static void UseErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                    var exceptionType = contextFeature.Error.GetType();

                    string message = "Internal Server Error";
                    var level = LogLevel.None;
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    switch (exceptionType)
                    {
                        case var type when type == typeof(ValidationException):
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            level = LogLevel.Information;
                            if (contextFeature.Error is ValidationException validationException)
                            {
                                message = string.Join(",", validationException.Errors.Select(e => e.ErrorMessage));
                            }

                            break;
                        default:
                            break;
                    }

                    logger.Log(level, "Something went wrong: {ContextFeatureError}", contextFeature.Error);

                    await context.Response.WriteAsJsonAsync(new { context.Response.StatusCode, Message = message });
                }
            });
        });
    }
}
