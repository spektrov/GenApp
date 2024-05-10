using System.IO.Compression;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class AppsettingsGenCommand(IFileGenService fileGenService, IConnectionDetailsProvider connectionProvider) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
         await fileGenService.CreateEntryAsync(
            archive,
            $"appsettings.json".ToApiProjectFile(model.AppName),
            new AppsettingsModel
            {
                ConnectionString = BuildConnectionString(model, model.DbmsType),
                MigrationFolderPath = model.UseDocker ? "/app/db" : "../../db",
                MigrationHistoryFilePath = model.UseDocker ? "/app/db/0_migration_history.txt" : "../../db/0_migration_history.txt",
            },
            token);
    }

    private string BuildConnectionString(ApplicationDataModel model, DbmsType dbmsType)
    {
        if (!model.UseDocker && string.IsNullOrEmpty(model.ConnectionString))
        {
            throw new ArgumentException("Cannot build connection string");
        }

        if (!string.IsNullOrEmpty(model.ConnectionString))
        {
            return model.ConnectionString;
        }

        var connection = connectionProvider.Get(model);

        string connStr = dbmsType switch
        {
            DbmsType.MYSQL => $"Server=localhost;Port=3306;Database={connection.DbName};Uid={connection.User};Pwd={connection.Password};",
            DbmsType.MSSQLSERVER => $"Server=localhost;Port=5433;Database={connection.DbName};User Id={connection.User};Password={connection.Password};",
            DbmsType.POSTGRESQL => $"Host=postgres-db;Port=5432;Database={connection.DbName};Username={connection.User};Password={connection.Password};",
            _ => throw new ArgumentException("Invalid DBMS type", nameof(dbmsType))
        };

        return connStr;
    }
}
