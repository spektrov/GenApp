using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
public class BootstrapGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public BootstrapGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new BootstrapGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        var startupModel = new ApiBootstrapModel
        {
            Namespace = $"{model.AppName}.API",
            Usings = new[]
            {
                "System.Text.Json.Serialization",
                "System.Text.Json",
                $"{model.AppName}.DAL.Models",
                $"{model.AppName}.API.Filters",
                $"{model.AppName}.DAL.CustomMigrations",
                $"{model.AppName}.DAL",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("Bootstrap.cs")),
                Arg.Is<ApiBootstrapModel>(x =>
                    x.Namespace == startupModel.Namespace &&
                    x.Usings.SequenceEqual(startupModel.Usings)),
                Arg.Any<CancellationToken>());
    }
}
