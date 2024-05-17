using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.SolutionLevel;
using GenApp.Templates.Resources.StaticTemplates;

namespace GenApp.DomainServices.Tests.CommandHandlers.SolutionLevel;
public class EditorconfigGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public EditorconfigGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new EditorconfigGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == ".editorconfig"),
                Arg.Is<string>(x => x == EditorconfigContent.Value),
                Arg.Any<CancellationToken>());
    }
}
