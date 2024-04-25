using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;

namespace GenApp.DomainServices.CommandHandlers.SolutionLevel;
internal class DbCustomMigrationsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            "0_migration_history.txt".ToDbFile(),
            string.Empty,
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            "1__Initial.sql".ToDbFile(),
            model.SqlTableScript,
            token);
    }
}
