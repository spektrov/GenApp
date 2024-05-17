using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.SolutionLevel;

namespace GenApp.DomainServices.Tests.CommandHandlers.SolutionLevel;
public class DbCustomMigrationsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public DbCustomMigrationsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new DbCustomMigrationsGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_migration_history()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == "db/0_migration_history.txt"),
                Arg.Is<string>(x => x == string.Empty),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_initial_migration()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == "db/1__Initial.sql"),
                Arg.Is<string>(x => x == model.SqlTableScript),
                Arg.Any<CancellationToken>());
    }
}
