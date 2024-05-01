using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class DomainModelsGenCommand(IFileGenService fileGenService, IMapper mapper, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities.AddIdFilter())
        {
            var properties = entity.Properties
                .Where(x => !x.IsForeignRelation && (model.Entities.HasId(x.Relation?.TargetEntity) || !x.IsNavigation))
                .OrderByDescending(x => x.IsId)
                .ThenBy(x => x.IsNavigation);
            var propertyDtos = mapper.Map<IEnumerable<DotnetPropertyDto>>(properties)
                .Select(AdjustProperty)
                .ToList();

            await fileGenService.CreateEntryAsync(
                archive,
                $"DomainModels/{entity.EntityName}Model.cs".ToBllProjectFile(model.AppName),
                new DomainModelModel
                {
                    Namespace = $"{model.AppName}.BLL.DomainModels",
                    ModelName = $"{entity.EntityName}Model",
                    Properties = propertyDtos,
                    Usings = Array.Empty<string>(),
                },
                token);
        }
    }

    private DotnetPropertyDto AdjustProperty(DotnetPropertyDto property)
    {
        property.Name = property.IsCollectionNavigation
            ? caseTransformer.ToPascalCase(caseTransformer.ToPlural(property.Name))
            : property.Name;

        property.Type = property.IsNavigation
            ? $"{property.Type}Model"
            : property.Type;

        return property;
    }
}
