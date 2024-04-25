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
    IValidator<StringGenSettingsDto> stringSettingsValidator,
    IValidator<FileGenSettingsDto> fileSettingsValidator) : ControllerBase
{
    [HttpPost]
    public async Task<Result<Stream>> GenerateWebAppAsync(
        [FromBody] StringGenSettingsDto genSettingsDto,
        CancellationToken token)
    {
        await stringSettingsValidator.ValidateAndThrowAsync(genSettingsDto, token);

        var settingsModel = mapper.Map<ApplicationDataModel>(genSettingsDto);

        AddContentHeader(settingsModel.AppName);

        return await solutionGenService.GenerateApplicationAsync(settingsModel, token);
    }

    [HttpPost("file")]
    public async Task<Result<Stream>> GenerateWebAppAsync(
        [FromForm] FileGenSettingsDto genSettingsDto,
        CancellationToken token)
    {
        await fileSettingsValidator.ValidateAndThrowAsync(genSettingsDto, token);

        var settingsModel = mapper.Map<ApplicationDataModel>(genSettingsDto);
        settingsModel.SqlTableScript = await ReadFileContentAsync(genSettingsDto.File);

        AddContentHeader(settingsModel.AppName);

        return await solutionGenService.GenerateApplicationAsync(settingsModel, token);
    }

    private void AddContentHeader(string appName)
    {
        Response.Headers.Append("Content-Disposition", $"attachment; filename={appName}.zip");
    }

    private async Task<string> ReadFileContentAsync(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        return await reader.ReadToEndAsync();
    }
}