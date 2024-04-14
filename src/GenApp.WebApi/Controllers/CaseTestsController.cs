using GenApp.Parsers.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CaseTestsController(ICaseTransformer caseTransformer) : ControllerBase
{
    [HttpGet]
    public ActionResult<string> TransformCase([FromQuery] string str, int option)
    {
        return option switch
        {
            1 => Ok(caseTransformer.ToCamelCase(str)),
            2 => Ok(caseTransformer.ToCamelUnderscoreCase(str)),
            3 => Ok(caseTransformer.ToSnakeCase(str)),
            4 => Ok(caseTransformer.ToPascalCase(str)),
            5 => Ok(caseTransformer.ToPlural(str)),
            _ => Ok(str),
        };
    }
}
