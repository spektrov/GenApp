using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class ModelsGenCommand(IFileGenService fileGenService, IMapper mapper, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities)
        {
            await GenerateEntityCreateRequest(archive, entity, model.AppName, token);
            await GenerateEntityUpdateRequest(archive, entity, model.AppName, token);
            await GenerateEntityResponse(archive, entity, model.AppName, token);
        }
    }

    private Task GenerateEntityCreateRequest(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var properties = entity.Properties.Where(x => !x.IsNavigation && !x.IsId);
        var propertyDtos = mapper.Map<IEnumerable<DotnetPropertyDto>>(properties).ToList();

        return fileGenService.CreateEntryAsync(
            archive,
            $"Models/Requests/{entity.EntityName}Requests/{entity.EntityName}CreateRequest.cs".ToApiProjectFile(appName),
            new CommandModelModel
            {
                Namespace = $"{appName}.API.Models.Requests.{entity.EntityName}Requests",
                ModelName = $"{entity.EntityName}CreateRequest",
                Properties = propertyDtos,
                Usings = Array.Empty<string>(),
            },
            token);
    }

    private Task GenerateEntityUpdateRequest(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var properties = entity.Properties.Where(x => !x.IsNavigation).OrderByDescending(x => x.IsId);
        var propertyDtos = mapper.Map<IEnumerable<DotnetPropertyDto>>(properties).ToList();

        return fileGenService.CreateEntryAsync(
            archive,
            $"Models/Requests/{entity.EntityName}Requests/{entity.EntityName}UpdateRequest.cs".ToApiProjectFile(appName),
            new CommandModelModel
            {
                Namespace = $"{appName}.API.Models.Requests.{entity.EntityName}Requests",
                ModelName = $"{entity.EntityName}UpdateRequest",
                Properties = propertyDtos,
                Usings = Array.Empty<string>(),
            },
            token);
    }

    private Task GenerateEntityResponse(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var properties = entity.Properties.OrderByDescending(x => x.IsId).ThenBy(x => x.IsNavigation);
        var propertyDtos = mapper.Map<IEnumerable<DotnetPropertyDto>>(properties)
            .Select(AdjustProperty).ToList();

        return fileGenService.CreateEntryAsync(
            archive,
            $"Models/Responses/{entity.EntityName}Response.cs".ToApiProjectFile(appName),
            new DomainModelModel
            {
                Namespace = $"{appName}.API.Models.Responses",
                ModelName = $"{entity.EntityName}Response",
                Properties = propertyDtos,
                Usings = Array.Empty<string>(),
            },
            token);
    }

    private DotnetPropertyDto AdjustProperty(DotnetPropertyDto property)
    {
        property.Name = property.IsCollectionNavigation
            ? caseTransformer.ToPascalCase(caseTransformer.ToPlural(property.Name))
            : property.Name;

        property.Type = property.IsNavigation
            ? $"{property.Type}Response"
            : property.Type;

        return property;
    }
}
