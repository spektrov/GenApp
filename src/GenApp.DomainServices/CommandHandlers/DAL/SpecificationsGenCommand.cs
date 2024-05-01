using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class SpecificationsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await GenerateStaticModels(archive, model, token);

        foreach (var entity in model.Entities.AddIdFilter())
        {
            await GenerateSearchSpecifications(archive, entity, model.AppName, token);
            await GenerateFilterSpecifications(archive, entity, model.AppName, token);
            await GenerateRangeSpecifications(archive, entity, model.AppName, token);
            await GenerateByIdSpecifications(archive, entity, model.AppName, token);
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
                    EntityName = $"{entity.EntityName}Entity",
                    KeyType = entity.IdType,
                    PropertyName = property.Name,
                    Usings = new List<string>
                    {
                        "System.Linq.Expressions",
                        "Microsoft.EntityFrameworkCore",
                        $"{appName}.DAL.Entities",
                    }.Order(),
                },
                token);
        }
    }

    private async Task GenerateFilterSpecifications(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var filterProperties = entity.Properties.Where(x => !x.IsId && DotnetFilterTypes.Filter.Contains(x.Type));
        if (!filterProperties.Any())
        {
            return;
        }

        foreach (var property in filterProperties)
        {
            var file = $"Specifications/{entity.EntityName}Specifications/Find{entity.EntityName}By{property.Name}.cs";

            await fileGenService.CreateEntryAsync(
                archive,
                file.ToDalProjectFile(appName),
                new EntityFilterSpecificationModel
                {
                    Namespace = $"{appName}.DAL.Specifications.{entity.EntityName}Specifications",
                    SpecificationName = $"Find{entity.EntityName}By{property.Name}",
                    EntityName = $"{entity.EntityName}Entity",
                    KeyType = entity.IdType,
                    PropertyName = property.Name,
                    PropertyType = property.Type,
                    IsNullable = !property.NotNull && property.Type != DotnetTypes.String,
                    Usings = new List<string>
                    {
                        "System.Linq.Expressions",
                        $"{appName}.DAL.Entities",
                    }.Order(),
                },
                token);
        }
    }

    private async Task GenerateRangeSpecifications(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var rangeProperties = entity.Properties.Where(x => !x.IsId && !x.IsForeignRelation && DotnetFilterTypes.Range.Contains(x.Type));
        if (!rangeProperties.Any())
        {
            return;
        }

        foreach (var property in rangeProperties)
        {
            var file = $"Specifications/{entity.EntityName}Specifications/Range{entity.EntityName}By{property.Name}.cs";

            await fileGenService.CreateEntryAsync(
                archive,
                file.ToDalProjectFile(appName),
                new EntityRangeSpecificationModel
                {
                    Namespace = $"{appName}.DAL.Specifications.{entity.EntityName}Specifications",
                    SpecificationName = $"Range{entity.EntityName}By{property.Name}",
                    EntityName = $"{entity.EntityName}Entity",
                    KeyType = entity.IdType,
                    PropertyName = property.Name,
                    PropertyType = property.Type,
                    IsNullable = !property.NotNull,
                    Usings = new List<string>
                    {
                        "System.Linq.Expressions",
                        $"{appName}.DAL.Entities",
                        $"{appName}.DAL.Models",
                    }.Order(),
                },
                token);
        }
    }

    private async Task GenerateByIdSpecifications(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        var property = entity.Properties.FirstOrDefault(x => x.IsId);
        if (property == null)
        {
            return;
        }

        var file = $"Specifications/{entity.EntityName}Specifications/Find{entity.EntityName}ById.cs";

        await fileGenService.CreateEntryAsync(
            archive,
            file.ToDalProjectFile(appName),
            new EntitySpecificationByIdModel
            {
                Namespace = $"{appName}.DAL.Specifications.{entity.EntityName}Specifications",
                SpecificationName = $"Find{entity.EntityName}ById",
                EntityName = $"{entity.EntityName}Entity",
                KeyType = property.Type,
                PropertyName = property.Name,
                Usings = new List<string>
                {
                    "System.Linq.Expressions",
                    $"{appName}.DAL.Entities",
                }.Order(),
            },
            token);
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
