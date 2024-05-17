using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.BLL;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.BLL;
public class StaticModelsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public StaticModelsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new StaticModelsGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_QueryParameters()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("QueryParameters.cs")),
                Arg.Is<QueryParametersModel>(x => x.Namespace == $"{model.AppName}.BLL.Models"),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_PaginationParameters()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("PaginationParameters.cs")),
                Arg.Is<PaginationParametersModel>(x => x.Namespace == $"{model.AppName}.BLL.Models"),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_PagedResponse()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("PagedResponse.cs")),
                Arg.Is<PagedResponseModel>(x => x.Namespace == $"{model.AppName}.BLL.Models"),
                Arg.Any<CancellationToken>());
    }
}
