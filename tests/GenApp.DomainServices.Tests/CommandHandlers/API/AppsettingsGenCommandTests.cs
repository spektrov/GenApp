using System.Data;
using System.IO.Compression;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.Parsers.CSharp.Mappers;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
public class AppsettingsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly IConnectionDetailsProvider _connectionDetailsProvider;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public AppsettingsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _connectionDetailsProvider = Substitute.For<IConnectionDetailsProvider>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new AppsettingsGenCommand(_fileGenService, _connectionDetailsProvider);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_postgresql_docker()
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

        var appsettingsModel = new AppsettingsModel
        {
            ConnectionString = $"Host=postgres-db;Port=5432;Database={connection.DbName};Username={connection.User};Password={connection.Password};",
            MigrationFolderPath = "/app/db",
            MigrationHistoryFilePath = "/app/db/0_migration_history.txt",
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("appsettings.json")),
                Arg.Is<AppsettingsModel>(x =>
                    x.ConnectionString == appsettingsModel.ConnectionString &&
                    x.MigrationFolderPath == appsettingsModel.MigrationFolderPath &&
                    x.MigrationHistoryFilePath == appsettingsModel.MigrationHistoryFilePath),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_postgresql_connection_string()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.POSTGRESQL)
            .With(x => x.UseDocker, false)
            .With(x => x.ConnectionString, "connection_string")
            .Create();

        var appsettingsModel = new AppsettingsModel
        {
            ConnectionString = model.ConnectionString,
            MigrationFolderPath = "../../db",
            MigrationHistoryFilePath = "../../db/0_migration_history.txt",
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("appsettings.json")),
                Arg.Is<AppsettingsModel>(x =>
                    x.ConnectionString == appsettingsModel.ConnectionString &&
                    x.MigrationFolderPath == appsettingsModel.MigrationFolderPath &&
                    x.MigrationHistoryFilePath == appsettingsModel.MigrationHistoryFilePath),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_mysql_docker()
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

        var appsettingsModel = new AppsettingsModel
        {
            ConnectionString = $"Server=mysql-db;Port=3306;Database={connection.DbName};Uid={connection.User};Pwd={connection.Password};",
            MigrationFolderPath = "/app/db",
            MigrationHistoryFilePath = "/app/db/0_migration_history.txt",
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("appsettings.json")),
                Arg.Is<AppsettingsModel>(x =>
                    x.ConnectionString == appsettingsModel.ConnectionString &&
                    x.MigrationFolderPath == appsettingsModel.MigrationFolderPath &&
                    x.MigrationHistoryFilePath == appsettingsModel.MigrationHistoryFilePath),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_sqlserver_connection_string()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MSSQLSERVER)
            .With(x => x.UseDocker, false)
            .With(x => x.ConnectionString, "connection_string")
            .Create();

        var appsettingsModel = new AppsettingsModel
        {
            ConnectionString = model.ConnectionString,
            MigrationFolderPath = "../../db",
            MigrationHistoryFilePath = "../../db/0_migration_history.txt",
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("appsettings.json")),
                Arg.Is<AppsettingsModel>(x =>
                    x.ConnectionString == appsettingsModel.ConnectionString &&
                    x.MigrationFolderPath == appsettingsModel.MigrationFolderPath &&
                    x.MigrationHistoryFilePath == appsettingsModel.MigrationHistoryFilePath),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_throw_exception_when_not_use_docker_and_no_conn_string()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MSSQLSERVER)
            .With(x => x.UseDocker, false)
            .Without(x => x.ConnectionString)
            .Create();

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.ExecuteAsync(_archive, model, default));
    }
}
