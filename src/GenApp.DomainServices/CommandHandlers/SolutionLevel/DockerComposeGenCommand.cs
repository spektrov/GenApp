using System.IO.Compression;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.DomainServices.CommandHandlers.SolutionLevel;
public class DockerComposeGenCommand(IFileGenService fileGenService, IConnectionDetailsProvider connectionProvider) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        if (!model.UseDocker || !string.IsNullOrEmpty(model.ConnectionString))
        {
            return Task.CompletedTask;
        }

        var connection = connectionProvider.Get(model);

        var dockerModel = model.DbmsType switch
        {
            DbmsType.MYSQL => GetForMySql(model.AppName, connection),
            DbmsType.POSTGRESQL => GetForPostgreSql(model.AppName, connection),
            _ => throw new ArgumentException("Invalid DBMS type"),
        };

        return fileGenService.CreateEntryAsync(
            archive,
            "docker-compose.yml".ToCoreSolutionFile(),
            dockerModel,
            token);
    }

    private DockerComposeModel GetForPostgreSql(string appName, ConnectionDetailsModel connection)
    {
        return new DockerComposeModel
        {
            DbServiceName = "postgres-db",
            DbImageName = "postgres",
            DbPorts = "5432:5432",
            VolumesValue = "./data:/var/lib/postgresql/data",
            DockerProjectName = $"{appName}.API",
            EnvVariables = new List<VariableDto>
            {
                new() { Name = "POSTGRES_PASSWORD", Value = $"{connection.Password}" },
                new() { Name = "POSTGRES_DB", Value = $"{connection.DbName}" },
            },
        };
    }

    private DockerComposeModel GetForMySql(string appName, ConnectionDetailsModel connection)
    {
        return new DockerComposeModel
        {
            DbServiceName = "mysql-db",
            DbImageName = "mysql:8.0.27",
            DbPorts = "3306:3306",
            VolumesValue = "~/apps/mysql:/var/lib/mysql",
            DockerProjectName = $"{appName}.API",
            EnvVariables = new List<VariableDto>
            {
                new() { Name = "MYSQL_ROOT_PASSWORD", Value = $"{connection.Password}" },
                new() { Name = "MYSQL_USER", Value = $"{connection.User}" },
                new() { Name = "MYSQL_PASSWORD", Value = $"{connection.Password}" },
                new() { Name = "MYSQL_DATABASE", Value = $"{connection.DbName}" },
            },
        };
    }
}
