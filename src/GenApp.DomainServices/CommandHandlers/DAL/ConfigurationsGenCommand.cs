using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class ConfigurationsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities)
        {
            await fileGenService.CreateEntryAsync(
                archive,
                $"Configurations/{entity.EntityName}Configuration.cs".ToDalProjectFile(model.AppName),
                new EntityConfigurationModel
                {
                    Namespace = $"{model.AppName}.DAL.Configurations",
                    ConfigurationName = $"{entity.EntityName}Configuration",
                    EntityName = $"{entity.EntityName}Entity",
                    TableName = model.Tables,
                    IdColumnName = ??,
                    HasPK = entity.Properties.Any(x => x.IsId),
                    Usings = new[]
                    {
                        "Microsoft.EntityFrameworkCore.Metadata.Builders",
                        "Microsoft.EntityFrameworkCore",
                        $"{model.AppName}.DAL.Entities"
                    }.Order(),
                },
                token);
        }
    }
}
