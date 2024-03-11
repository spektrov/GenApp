using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers;
internal class CustomMigrationsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            "CustomMigrations/CustomDbMigrator.cs".ToDalProjectFile(model.AppName),
            new CustomMigrationsClassModel
            {
                Namespace = $"{model.AppName}.DAL.CustomMigrations",
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            "CustomMigrations/ICustomDbMigrator.cs".ToDalProjectFile(model.AppName),
            new CustomMigrationsInterfaceModel
            {
                Namespace = $"{model.AppName}.DAL.CustomMigrations",
            },
            token);
    }
}
