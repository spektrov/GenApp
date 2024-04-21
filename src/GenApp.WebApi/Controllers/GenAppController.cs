using AutoMapper;
using FluentResults;
using FluentValidation;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenAppController(
    IMapper mapper,
    ISolutionGenService solutionGenService,
    IValidator<GenSettingsDto> validator) : ControllerBase
{
    [HttpPost]
    public async Task<Result<Stream>> GenerateWebApiAsync(
        [FromBody] GenSettingsDto genSettingsDto,
        CancellationToken token)
    {
        await validator.ValidateAndThrowAsync(genSettingsDto, token);

        var settingsModel = mapper.Map<ApplicationDataModel>(genSettingsDto);
        Response.Headers.Append("Content-Disposition", $"attachment; filename={settingsModel.AppName}.zip");
        return await solutionGenService.GenerateApplicationAsync(settingsModel, token);
    }
}