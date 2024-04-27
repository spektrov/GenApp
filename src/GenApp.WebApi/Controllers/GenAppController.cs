using AutoMapper;
using FluentResults;
using FluentValidation;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.WebApi.ActionFilters;
using GenApp.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenApp.WebApi.Controllers;

[Route("api/genapp")]
[ApiController]
public class GenAppController(
    IMapper mapper,
    ISolutionGenService solutionGenService,
    IValidator<StringGenSettingsDto> stringSettingsValidator,
    IValidator<FileGenSettingsDto> fileSettingsValidator) : ControllerBase
{
    [AddContentHeader]
    [HttpPost("text")]
    public async Task<Result<Stream>> GenerateWebAppAsync(
        [FromBody] StringGenSettingsDto genSettingsDto,
        CancellationToken token)
    {
        await stringSettingsValidator.ValidateAndThrowAsync(genSettingsDto, token);

        var settingsModel = mapper.Map<ApplicationDataModel>(genSettingsDto);

        return await solutionGenService.GenerateApplicationAsync(settingsModel, token);
    }

    [AddContentHeader]
    [HttpPost("file")]
    public async Task<Result<Stream>> GenerateWebAppAsync(
        [FromForm] FileGenSettingsDto genSettingsDto,
        CancellationToken token)
    {
        await fileSettingsValidator.ValidateAndThrowAsync(genSettingsDto, token);

        var settingsModel = mapper.Map<ApplicationDataModel>(genSettingsDto);
        settingsModel.SqlTableScript = await ReadFileContentAsync(genSettingsDto.File);

        return await solutionGenService.GenerateApplicationAsync(settingsModel, token);
    }

    private async Task<string> ReadFileContentAsync(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        return await reader.ReadToEndAsync();
    }
}