using AutoMapper;
using FluentResults;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenAppController(IMapper mapper, ISolutionGenService solutionGenService) : ControllerBase
{
    [HttpPost]
    public async Task<Result<Stream>> GenerateWebApiAsync(
        [FromBody] GenSettingsDto genSettingsDto,
        CancellationToken token)
    {
        var settingsModel = mapper.Map<GenSettingsModel>(genSettingsDto);
        Response.Headers.Append("Content-Disposition", $"attachment; filename={settingsModel.AppName}.zip");
        return await solutionGenService.GenerateApplicationAsync(settingsModel, token);
    }
}