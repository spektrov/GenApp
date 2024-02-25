using System.Net.Mime;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GenApp.WebApi.ActionFilters;

public class ResultActionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult { Value: ResultBase result })
        {
            var value = result.IsFailed ? null : GetValue(result);
            context.Result = result switch
            {
                { IsSuccess: true } when value is null => new NoContentResult(),
                { IsSuccess: true } when value is Stream => new FileStreamResult(value as Stream, MediaTypeNames.Application.Zip),
                { IsSuccess: true } => new OkObjectResult(value),
                _ when !context.ModelState.IsValid => new UnprocessableEntityObjectResult(context.ModelState),
                { IsFailed: true } when result.Errors.Count == 1 => new BadRequestObjectResult(result.Errors[0].Message),
                _ => new BadRequestObjectResult(result.Reasons.Select(r => r.Message).ToArray()),
            };
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Method intentionally left empty.
    }

    private static object GetValue(ResultBase result)
    {
        return result is IResult<object> objResult ? objResult.Value : null;
    }
}
