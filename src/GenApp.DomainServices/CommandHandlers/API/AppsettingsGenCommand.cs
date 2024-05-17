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
                ConnectionString = BuildConnectionString(model),
                MigrationFolderPath = model.UseDocker ? "/app/db" : "../../db",
                MigrationHistoryFilePath = model.UseDocker ? "/app/db/0_migration_history.txt" : "../../db/0_migration_history.txt",
            },
            token);
    }

    private string BuildConnectionString(ApplicationDataModel model)
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
        var serverName = GetServerName(model);

        string connStr = model.DbmsType switch
        {
            DbmsType.MYSQL => $"Server={serverName};Port=3306;Database={connection.DbName};Uid={connection.User};Pwd={connection.Password};",
            DbmsType.MSSQLSERVER => $"Server={serverName};Port=5433;Database={connection.DbName};User Id={connection.User};Password={connection.Password};",
            DbmsType.POSTGRESQL => $"Host={serverName};Port=5432;Database={connection.DbName};Username={connection.User};Password={connection.Password};",
            _ => throw new ArgumentException("Invalid DBMS type", nameof(model.DbmsType))
        };

        return connStr;
    }

    private string GetServerName(ApplicationDataModel model)
    {
        var serverName = model.DbmsType switch
        {
            DbmsType.MYSQL => model.UseDocker ? "mysql-db" : "localhost",
            DbmsType.MSSQLSERVER => "localhost",
            DbmsType.POSTGRESQL => model.UseDocker ? "postgres-db" : "localhost",
            _ => throw new ArgumentException("Invalid DBMS type", nameof(model.DbmsType))
        };

        return serverName;
    }
}
