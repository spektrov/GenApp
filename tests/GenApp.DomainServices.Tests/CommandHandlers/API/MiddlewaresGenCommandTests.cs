using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
public class MiddlewaresGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public MiddlewaresGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new MiddlewaresGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters()
    {
        var model = _fixture.Create<ApplicationDataModel>();

        var middlewareModel = new ExceptionMiddlewareExtensionModel
        {
            Namespace = $"{model.AppName}.API.Middlewares",
            Usings = new List<string>
            {
                "Microsoft.AspNetCore.Diagnostics",
                "System.Net.Mime",
                $"{model.AppName}.BLL.Exceptions",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("ExceptionMiddlewareExtension.cs")),
                Arg.Is<ExceptionMiddlewareExtensionModel>(x =>
                    x.Namespace == middlewareModel.Namespace &&
                    x.Usings.SequenceEqual(middlewareModel.Usings)),
                Arg.Any<CancellationToken>());
    }
}
