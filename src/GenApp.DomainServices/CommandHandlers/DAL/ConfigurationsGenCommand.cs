using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class ConfigurationsGenCommand(IFileGenService fileGenService, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities)
        {
            await GenerateConfiguration(archive, entity, model.AppName, token);
        }
    }

    private Task GenerateConfiguration(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
            archive,
            $"Configurations/{entity.EntityName}Configuration.cs".ToDalProjectFile(appName),
            new EntityConfigurationModel
            {
                Namespace = $"{appName}.DAL.Configurations",
                ConfigurationName = $"{entity.EntityName}Configuration",
                EntityName = $"{entity.EntityName}Entity",
                TableName = entity.Table.Name,
                IdColumnName = entity.Table.KeyName ?? string.Empty,
                HasPK = entity.HasId,
                ColumnConfigs = GetColumnConfigs(entity).ToList(),
                RelationConfigs = GetRelationConfigs(entity).ToList(),
                Usings = new[]
                {
                    "Microsoft.EntityFrameworkCore.Metadata.Builders",
                    "Microsoft.EntityFrameworkCore",
                    $"{appName}.DAL.Entities",
                }.Order(),
            },
            token);
    }

    private IEnumerable<ColumnConfigurationDto> GetColumnConfigs(DotnetEntityConfigurationModel entity)
    {
        var columnConfigs = entity.Properties
            .Where(x => !x.IsNavigation && !x.IsId)
            .Select(x => new ColumnConfigurationDto
            {
                PropertyName = x.Name,
                ColumnName = x.ColumnName,
                ConfigName = $"{x.Name}Column",
            });

        return columnConfigs;
    }

    private IEnumerable<RelationConfigurationDto> GetRelationConfigs(DotnetEntityConfigurationModel entity)
    {
        var reletionConfigs = entity.Properties
            .Where(x => x.IsNavigation && x.Relation is { IsReverted: false })
            .Select(x => new RelationConfigurationDto
            {
                NavigationPropertyName = x.Name,
                Cardinality = x.Relation!.IsOneToOne ? "WithOne" : "WithMany",
                RevertedPropertyName = x.Relation.IsOneToOne
                    ? x.Relation.RevertedPropertyName
                    : caseTransformer.ToPascalCase(caseTransformer.ToPlural(x.Relation.RevertedPropertyName)),
                ForeignPropertyName = x.Relation.ForeignPropertyName,
                IsRequired = x.Relation.IsRequired ? "true" : "false",
                OnDeleteAction = x.Relation.OnDeleteAction,
            });

        return reletionConfigs;
    }
}
