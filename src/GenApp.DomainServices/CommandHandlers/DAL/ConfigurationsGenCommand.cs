using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class ConfigurationsGenCommand(IFileGenService fileGenService) : IGenCommand
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
        var columnConfigs = entity.Properties
            .Where(x => !x.IsNavigation && !x.IsId)
            .Select(x => new ColumnConfigurationDto
            {
                PropertyName = x.Name,
                ColumnName = x.ColumnName,
                ConfigName = $"{x.Name}Column",
            }).ToList();

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
                ColumnConfigs = columnConfigs,
                Usings = new[]
                {
                    "Microsoft.EntityFrameworkCore.Metadata.Builders",
                    "Microsoft.EntityFrameworkCore",
                    $"{appName}.DAL.Entities",
                }.Order(),
            },
            token);
    }
}
