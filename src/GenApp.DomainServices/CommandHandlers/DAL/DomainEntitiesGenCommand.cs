using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class DomainEntitiesGenCommand(IFileGenService fileGenService, IMapper mapper, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var usingList = new List<string> { $"{model.AppName}.DAL.Interfaces" };
        foreach (var entity in model.Entities)
        {
            var fileName = $"Entities/{entity.EntityName}Entity.cs";
            var properties = entity.Properties.OrderByDescending(x => x.IsId).ThenBy(x => x.IsNavigation);
            var propertyDtos = mapper.Map<IEnumerable<DotnetPropertyDto>>(properties)
                .Select(AdjustProperty).ToList();

            await fileGenService.CreateEntryAsync(
                archive,
                fileName.ToDalProjectFile(model.AppName),
                new DomainEntityModel
                {
                    Namespace = $"{model.AppName}.DAL.Entities",
                    EntityName = $"{entity.EntityName}Entity",
                    KeyType = entity.Properties.FirstOrDefault(x => x.IsId)?.Type,
                    Properties = propertyDtos,
                    Usings = usingList,
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
            ? $"{property.Type}Entity"
            : property.Type;

        return property;
    }
}
