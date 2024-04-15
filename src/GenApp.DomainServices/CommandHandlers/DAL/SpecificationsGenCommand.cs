using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class SpecificationsGenCommand(IFileGenService fileGenService, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await GenerateStaticModels(archive, model, token);

        foreach (var entity in model.Entities)
        {
            // await GenerateSearchSpecifications(archive, entity, model.AppName, token);
        }
    }

    private async Task GenerateSearchSpecifications(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var searchProperties = entity.Properties.Where(x => !x.IsId && DotnetFilterTypes.Search.Contains(x.Type));
        if (!searchProperties.Any())
        {
            return;
        }

        foreach (var property in searchProperties)
        {
            var file = $"Specifications/{entity.EntityName}Specifications/Search{entity.EntityName}By{property.Name}.cs";

            await fileGenService.CreateEntryAsync(
                archive,
                file.ToDalProjectFile(appName),
                new EntitySearchSpecificationModel
                {
                    Namespace = $"{appName}.DAL.Specifications.{entity.EntityName}Specifications",
                    SpecificationName = $"Search{entity.EntityName}By{property.Name}",
                    EntityName = entity.EntityName,
                    KeyType = entity.Properties.FirstOrDefault(x => x.IsId)?.Type,
                    PropertyName = property.Name,
                },
                token);
        }
    }

    private async Task GenerateStaticModels(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            $"Specifications/Specification.cs".ToDalProjectFile(model.AppName),
            new SpecificationModel
            {
                Namespace = $"{model.AppName}.DAL.Specifications",
                Usings = new List<string>
                {
                    "System.Linq.Expressions",
                    $"{model.AppName}.DAL.Interfaces",
                    $"{model.AppName}.DAL.Specifications.Orerators",
                }.Order(),
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            $"Specifications/Orerators/AndSpecification.cs".ToDalProjectFile(model.AppName),
            new AndSpecificationModel
            {
                Namespace = $"{model.AppName}.DAL.Specifications.Orerators",
                Usings = new List<string>
                {
                    "System.Linq.Expressions",
                    $"{model.AppName}.DAL.Interfaces",
                }.Order(),
            },
            token);

        await fileGenService.CreateEntryAsync(
             archive,
             $"Specifications/Orerators/OperationExtensions.cs".ToDalProjectFile(model.AppName),
             new OperationExtensionsModel
             {
                 Namespace = $"{model.AppName}.DAL.Specifications.Orerators",
                 Usings = new List<string>
                 {
                    "System.Linq.Expressions",
                    $"{model.AppName}.DAL.Interfaces",
                 }.Order(),
             },
             token);

        await fileGenService.CreateEntryAsync(
            archive,
            $"Specifications/Orerators/TrueSpecification.cs".ToDalProjectFile(model.AppName),
            new TrueSpecificationModel
            {
                Namespace = $"{model.AppName}.DAL.Specifications.Orerators",
                Usings = new List<string>
                {
                    "System.Linq.Expressions",
                    $"{model.AppName}.DAL.Interfaces",
                }.Order(),
            },
            token);
    }
}
