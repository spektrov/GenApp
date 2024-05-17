using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
public class ProgramGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public ProgramGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new ProgramGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        var startupModel = new ProgramModel
        {
            EnableDocker = true,
            Usings = new List<string>
            {
                $"{model.AppName}.API",
                $"{model.AppName}.API.Middlewares",
                $"{model.AppName}.BLL",
                $"{model.AppName}.DAL",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("Program.cs")),
                Arg.Is<ProgramModel>(x =>
                    x.Namespace == startupModel.Namespace &&
                    x.EnableDocker == startupModel.EnableDocker &&
                    x.Usings.SequenceEqual(startupModel.Usings)),
                Arg.Any<CancellationToken>());
    }
}
