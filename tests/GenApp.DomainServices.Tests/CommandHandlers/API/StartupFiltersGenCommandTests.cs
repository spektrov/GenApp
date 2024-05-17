using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
public class StartupFiltersGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public StartupFiltersGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new StartupFiltersGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        var startupModel = new MigrationStartupFilterModel
        {
            Namespace = $"{model.AppName}.API.Filters",
            Usings = new List<string>
            {
                "Microsoft.EntityFrameworkCore",
                $"{model.AppName}.DAL.CustomMigrations",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("MigrationStartupFilter.cs")),
                Arg.Is<MigrationStartupFilterModel>(x =>
                    x.Namespace == startupModel.Namespace &&
                    x.Usings.SequenceEqual(startupModel.Usings)),
                Arg.Any<CancellationToken>());
    }
}
