using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class HelpersGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public HelpersGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new HelpersGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_PageCountCalculator()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("PageCountCalculator")),
                Arg.Is<PageCountCalcucatorModel>(x =>
                    x.Namespace == $"{model.AppName}.DAL.Helpers" &&
                    !x.Usings!.Any()),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_SortingOrderCalculator()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("SortingOrderCalculator")),
                Arg.Is<SortingOrderCalculatorModel>(x =>
                    x.Namespace == $"{model.AppName}.DAL.Helpers" &&
                    x.Usings!.SequenceEqual(new[] { $"{model.AppName}.DAL.Enums" })),
                Arg.Any<CancellationToken>());
    }
}
