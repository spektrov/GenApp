using System.IO.Compression;
using GenApp.Domain.Enums;
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
                ConnectionString = BuildConnectionString(model, model.DbmsType),
            },
            token);
    }

    private string BuildConnectionString(ApplicationDataModel model, DbmsType dbmsType)
    {
        var user = "admin";
        var password = Guid.NewGuid().ToString();
        var dbName = model.AppName.ToLower();

        string connStr = dbmsType switch
        {
            DbmsType.MYSQL => $"Server=localhost;Port=3306;Database={dbName};Uid={user};Pwd={password};",
            DbmsType.MSSQLSERVER => $"Server=localhost;Port=5433;Database={dbName};User Id={user};Password={password};",
            DbmsType.POSTGRESQL => $"Host=localhost;Port=5432;Database={dbName};Username={user};Password={password};",
            _ => throw new ArgumentException("Invalid DBMS type", nameof(dbmsType))
        };

        return connStr;
    }
}
