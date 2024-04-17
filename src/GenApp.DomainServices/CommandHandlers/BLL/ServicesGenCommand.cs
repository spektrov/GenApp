using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class ServicesGenCommand(IFileGenService fileGenService, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities)
        {
            var fileName = $"Services/{entity.EntityName}Service.cs";

            await fileGenService.CreateEntryAsync(
                archive,
                fileName.ToBllProjectFile(model.AppName),
                new ServiceModel
                {
                    Namespace = $"{model.AppName}.BLL.Services",
                    ServiceName = $"{entity.EntityName}Service",
                    InterfaceName = $"I{entity.EntityName}Service",
                    RepositoryInterfaceName = $"I{entity.EntityName}Repository",
                    ModelName = $"{entity.EntityName}Model",
                    EntityName = $"{entity.EntityName}Entity",
                    CommandModelName = $"{entity.EntityName}CommandModel",
                    FindByIdSpecification = $"Find{entity.EntityName}ById",
                    KeyType = entity.Properties.FirstOrDefault(x => x.IsId)?.Type ?? string.Empty,
                    GetManyIncludes = GetManyIncludes(entity),
                    GetByIdIncludes = GetByIdIncludes(entity),
                    PropertiesForUpdate = GetPropertiesForUpdate(entity),
                    Usings = new List<string>
                    {
                        "AutoMapper",
                        "Microsoft.Extensions.Logging",
                        $"{model.AppName}.BLL.CommandModels",
                        $"{model.AppName}.BLL.DomainModels",
                        $"{model.AppName}.BLL.Exceptions",
                        $"{model.AppName}.BLL.Interfaces",
                        $"{model.AppName}.BLL.Models",
                        $"{model.AppName}.DAL.Entities",
                        $"{model.AppName}.DAL.Helpers",
                        $"{model.AppName}.DAL.Interfaces",
                        $"{model.AppName}.DAL.Models",
                        $"{model.AppName}.DAL.Specifications.{entity.EntityName}Specifications",
                    }.Order(),
                },
                token);
        }
    }

    private ICollection<string> GetPropertiesForUpdate(DotnetEntityConfigurationModel entity)
    {
        var propertiesForUpdate = entity.Properties
            .Where(x => !x.IsId && !x.IsNavigation)
            .Select(x => x.Name)
            .ToList();

        return propertiesForUpdate;
    }

    private ICollection<string> GetManyIncludes(DotnetEntityConfigurationModel entity)
    {
        return GetIncludes(entity, x => x.Relation.IsOneToOne || x.Relation.IsReverted);
    }

    private ICollection<string> GetByIdIncludes(DotnetEntityConfigurationModel entity)
    {
        return GetIncludes(entity, x => true);
    }

    private ICollection<string> GetIncludes(DotnetEntityConfigurationModel entity, Func<DotnetPropertyConfigurationModel, bool> predicate)
    {
        return entity.Properties
            .Where(x => x.IsNavigation && x.Relation != null && predicate(x))
            .Select(x => $"{entity.EntityName}Entity.{GetPropertyName(x.Name, x.Relation!.IsOneToOne || x.Relation.IsReverted)}")
            .ToList();
    }

    private string GetPropertyName(string name, bool isOneToOne)
    {
        if (isOneToOne)
        {
            return name;
        }

        return caseTransformer.ToPascalCase(caseTransformer.ToPlural(name));
    }
}
