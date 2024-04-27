using Microsoft.AspNetCore.Mvc.Filters;

namespace GenApp.WebApi.ActionFilters;

public class AddContentHeaderAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var filename = DateTime.Now.ToString();
        context.HttpContext.Response.Headers.Append("Content-Disposition", $"attachment; filename={filename}.zip");
    }
}
