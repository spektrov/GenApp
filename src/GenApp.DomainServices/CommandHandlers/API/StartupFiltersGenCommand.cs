using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class StartupFiltersGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            "Filters/MigrationStartupFilter.cs".ToApiProjectFile(model.AppName),
            new MigrationStartupFilterModel
            {
                Namespace = $"{model.AppName}.API.Filters",
                Usings = new List<string>
                {
                    "Microsoft.EntityFrameworkCore",
                    $"{model.AppName}.DAL.CustomMigrations",
                }.Order(),
            },
            token);
    }
}
