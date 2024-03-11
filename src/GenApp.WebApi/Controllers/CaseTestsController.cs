using GenApp.DomainServices.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GenApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CaseTestsController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> TransformCase([FromQuery] string str, int option)
    {
        return option switch
        {
            1 => Ok(str.ToCamelCase()),
            2 => Ok(str.ToCamelUnderscoreCase()),
            3 => Ok(str.ToSnakeCase()),
            4 => Ok(str.ToPascalCase()),
            5 => Ok(str.ToPlural()),
            _ => Ok(str),
        };
    }
}
