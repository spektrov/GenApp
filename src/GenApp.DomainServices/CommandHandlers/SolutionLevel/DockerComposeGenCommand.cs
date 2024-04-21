using System.IO.Compression;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.DomainServices.CommandHandlers.SolutionLevel;
public class DockerComposeGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        if (!model.UseDocker)
        {
            return Task.CompletedTask;
        }

        var dockerModel = model.DbmsType switch
        {
            DbmsType.MYSQL => GetForMySql(model.AppName),
            DbmsType.MSSQLSERVER => GetForMsSqlServer(model.AppName),
            DbmsType.POSTGRESQL => GetForPosgreSql(model.AppName),
            _ => throw new ArgumentException("Invalid DBMS type"),
        };

        return fileGenService.CreateEntryAsync(
            archive,
            "docker-compose.yml".ToCoreSolutionFile(),
            dockerModel,
            token);
    }

    private DockerComposeModel GetForPosgreSql(string appName)
    {
        return new DockerComposeModel
        {
            DbServiceName = "postgres-db",
            DbImageName = "postgres:15.1-alpine",
            DbPorts = "5432:5432",
            VolumesValue = "~/apps/postgres:/var/lib/postgresql/data",
            DockerProjectName = $"{appName}.API",
            EnvVariables = new List<VariableDto>
            {
                new() { Name = "POSTGRES_PASSWORD", Value = "" },
                new() { Name = "POSTGRES_USER", Value = "" },
                new() { Name = "POSTGRES_DB", Value = "" },
            },
        };
    }

    private DockerComposeModel GetForMySql(string appName)
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
                new() { Name = "MYSQL_ROOT_PASSWORD", Value = "" },
                new() { Name = "MYSQL_USER", Value = "" },
                new() { Name = "MYSQL_PASSWORD", Value = "" },
                new() { Name = "MYSQL_DATABASE", Value = "" },
            },
        };
    }

    private DockerComposeModel GetForMsSqlServer(string appName)
    {
        return new DockerComposeModel
        {
            DbServiceName = "sqlserver-db",
            DbImageName = "mcr.microsoft.com/mssql/server:2019-latest",
            DbPorts = "5433:1433",
            VolumesValue = "~/apps/sqlserver:/var/opt/mssql",
            DockerProjectName = $"{appName}.API",
            EnvVariables = new List<VariableDto>
            {
                new() { Name = "SA_PASSWORD", Value = "" },
                new() { Name = "ACCEPT_EULA", Value = "Y" },
            },
        };
    }
}
