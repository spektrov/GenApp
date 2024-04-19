using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class AppsettingsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
         await fileGenService.CreateEntryAsync(
            archive,
            $"appsettings.json".ToApiProjectFile(model.AppName),
            new AppsettingsModel
            {
                ConnectionString = BuildConnectionString(model),
            },
            token);
    }

    private string BuildConnectionString(ApplicationDataModel model)
    {
        var user = "user";
        var password = Guid.NewGuid().ToString();
        var dbName = model.AppName.ToLower();

        var connStr = $"Host=localhost;Port=5432;Database={dbName};Username={user};Password={password}";
        return connStr;
    }
}
