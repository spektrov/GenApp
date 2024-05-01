using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class FilterModelsGenCommand(IFileGenService fileGenService, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await GenerateStaticModels(archive, model, token);

        foreach (var entity in model.Entities.AddIdFilter())
        {
            await GenerateSearchParameters(archive, entity, model.AppName, token);
            await GenerateFilterParameters(archive, entity, model.AppName, token);
            await GenerateRangeParameters(archive, entity, model.AppName, token);
        }
    }

    private Task GenerateSearchParameters(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var searchProperties = entity.Properties.Where(x => !x.IsId && DotnetFilterTypes.Search.Contains(x.Type));
        if (!searchProperties.Any())
        {
            return Task.CompletedTask;
        }

        var file = $"Models/{entity.EntityName}Models/{entity.EntityName}SearchParameters.cs";
        var properties = searchProperties.Select(x =>
            new FilterPropertyDto
            {
                FilterName = x.Name,
                JsonName = caseTransformer.ToSnakeCase(x.Name),
                Type = x.Type,
                IsSearch = true,
            }).ToList();

        return fileGenService.CreateEntryAsync(
            archive,
            file.ToDalProjectFile(appName),
            new EntityFilterParametersModel
            {
                Namespace = $"{appName}.DAL.Models.{entity.EntityName}Models",
                Name = $"{entity.EntityName}SearchParameters",
                Properties = properties,
            },
            token);
    }

    private Task GenerateRangeParameters(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var rangeProperties = entity.Properties.Where(x => !x.IsId && DotnetFilterTypes.Range.Contains(x.Type));
        if (!rangeProperties.Any())
        {
            return Task.CompletedTask;
        }

        var searchFile = $"Models/{entity.EntityName}Models/{entity.EntityName}RangeParameters.cs";
        var properties = rangeProperties.Select(x =>
            new FilterPropertyDto
            {
                FilterName = x.Name,
                JsonName = caseTransformer.ToSnakeCase(x.Name),
                Type = x.Type,
                IsRange = true,
            }).ToList();

        return fileGenService.CreateEntryAsync(
            archive,
            searchFile.ToDalProjectFile(appName),
            new EntityFilterParametersModel
            {
                Namespace = $"{appName}.DAL.Models.{entity.EntityName}Models",
                Name = $"{entity.EntityName}RangeParameters",
                Properties = properties,
            },
            token);
    }

    private Task GenerateFilterParameters(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var filterProperties = entity.Properties.Where(x => !x.IsId && DotnetFilterTypes.Filter.Contains(x.Type));
        if (!filterProperties.Any())
        {
            return Task.CompletedTask;
        }

        var searchFile = $"Models/{entity.EntityName}Models/{entity.EntityName}FilterParameters.cs";
        var properties = filterProperties.Select(x =>
            new FilterPropertyDto
            {
                FilterName = caseTransformer.ToPascalCase(caseTransformer.ToPlural(x.Name)),
                JsonName = caseTransformer.ToSnakeCase(caseTransformer.ToPlural(x.Name)),
                Type = x.Type,
                IsFilter = true,
            }).ToList();

        return fileGenService.CreateEntryAsync(
            archive,
            searchFile.ToDalProjectFile(appName),
            new EntityFilterParametersModel
            {
                Namespace = $"{appName}.DAL.Models.{entity.EntityName}Models",
                Name = $"{entity.EntityName}FilterParameters",
                Properties = properties,
            },
            token);
    }

    private async Task GenerateStaticModels(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            $"Models/FilterParameters.cs".ToDalProjectFile(model.AppName),
            new FilterParametersModel
            {
                Namespace = $"{model.AppName}.DAL.Models",
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            $"Models/AppJsonSerializerOptions.cs".ToDalProjectFile(model.AppName),
            new AppJsonSerializerOptionsModel
            {
                Namespace = $"{model.AppName}.DAL.Models",
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            $"Models/RangeParameters.cs".ToDalProjectFile(model.AppName),
            new RangeParametersModel
            {
                Namespace = $"{model.AppName}.DAL.Models",
            },
            token);
    }
}
