using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.SolutionLevel;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.SolutionLevel;
public class DockerComposeGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly IConnectionDetailsProvider _connectionDetailsProvider;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public DockerComposeGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _connectionDetailsProvider = Substitute.For<IConnectionDetailsProvider>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new DockerComposeGenCommand(_fileGenService, _connectionDetailsProvider);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_postgresql()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.POSTGRESQL)
            .With(x => x.UseDocker, true)
            .Without(x => x.ConnectionString)
            .Create();

        var connection = new ConnectionDetailsModel() { DbName = "coolapp", Password = Guid.NewGuid().ToString(), User = "postgres" };
        _connectionDetailsProvider.Get(model).Returns(connection);

        var dockerFile = new DockerComposeModel
        {
            DbServiceName = "postgres-db",
            DbImageName = "postgres",
            DbPorts = "5432:5432",
            VolumesValue = "./data:/var/lib/postgresql/data",
            DockerProjectName = $"CoolApp.API",
            EnvVariables = new List<VariableDto>
            {
                new() { Name = "POSTGRES_PASSWORD", Value = $"{connection.Password}" },
                new() { Name = "POSTGRES_DB", Value = $"{connection.DbName}" },
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == "docker-compose.yml"),
                Arg.Is<DockerComposeModel>(x =>
                    x.DbServiceName == dockerFile.DbServiceName &&
                    x.DbImageName == dockerFile.DbImageName &&
                    x.DbPorts == dockerFile.DbPorts &&
                    x.VolumesValue == dockerFile.VolumesValue &&
                    x.DockerProjectName == dockerFile.DockerProjectName &&
                    x.EnvVariables.SequenceEqual(dockerFile.EnvVariables)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_mysql()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MYSQL)
            .With(x => x.UseDocker, true)
            .Without(x => x.ConnectionString)
            .Create();

        var connection = new ConnectionDetailsModel() { DbName = "coolapp", Password = Guid.NewGuid().ToString(), User = "admin" };
        _connectionDetailsProvider.Get(model).Returns(connection);

        var dockerFile = new DockerComposeModel
        {
            DbServiceName = "mysql-db",
            DbImageName = "mysql:8.0.27",
            DbPorts = "3306:3306",
            VolumesValue = "~/apps/mysql:/var/lib/mysql",
            DockerProjectName = $"CoolApp.API",
            EnvVariables = new List<VariableDto>
            {
                new() { Name = "MYSQL_ROOT_PASSWORD", Value = $"{connection.Password}" },
                new() { Name = "MYSQL_USER", Value = $"{connection.User}" },
                new() { Name = "MYSQL_PASSWORD", Value = $"{connection.Password}" },
                new() { Name = "MYSQL_DATABASE", Value = $"{connection.DbName}" },
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == "docker-compose.yml"),
                Arg.Is<DockerComposeModel>(x =>
                    x.DbServiceName == dockerFile.DbServiceName &&
                    x.DbImageName == dockerFile.DbImageName &&
                    x.DbPorts == dockerFile.DbPorts &&
                    x.VolumesValue == dockerFile.VolumesValue &&
                    x.DockerProjectName == dockerFile.DockerProjectName &&
                    x.EnvVariables.SequenceEqual(dockerFile.EnvVariables)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_not_call_CreateEntryAsync_is_not_useDocker()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.UseDocker, false)
            .Without(x => x.ConnectionString)
            .Create();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .DidNotReceive()
            .CreateEntryAsync(Arg.Any<ZipArchive>(), Arg.Any<string>(), Arg.Any<DockerComposeModel>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_not_call_CreateEntryAsync_if_connection_string()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.ConnectionString)
            .Create();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .DidNotReceive()
            .CreateEntryAsync(Arg.Any<ZipArchive>(), Arg.Any<string>(), Arg.Any<DockerComposeModel>(), Arg.Any<CancellationToken>());
    }
}
