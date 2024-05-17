using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class CustomMigrationsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public CustomMigrationsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new CustomMigrationsGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_migration_class()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("CustomMigrations/CustomDbMigrator.cs")),
                Arg.Is<CustomMigrationsClassModel>(x => x.Namespace == $"{model.AppName}.DAL.CustomMigrations"),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_migration_interface()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("CustomMigrations/ICustomDbMigrator.cs")),
                Arg.Is<CustomMigrationsInterfaceModel>(x => x.Namespace == $"{model.AppName}.DAL.CustomMigrations"),
                Arg.Any<CancellationToken>());
    }
}
