using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class QueryableExtensionsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public QueryableExtensionsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new QueryableExtensionsGenCommand(_fileGenService);
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
                Arg.Is<string>(x => x.Contains("QueryableExtensions")),
                Arg.Is<QueryableExtensionsModel>(x =>
                    x.Namespace == $"{model.AppName}.DAL.Extensions" &&
                    x.Usings.SequenceEqual(
                        new[]
                        {
                           "System.Linq.Expressions",
                           "Microsoft.EntityFrameworkCore",
                           $"{model.AppName}.DAL.Enums",
                           $"{model.AppName}.DAL.Interfaces",
                           $"{model.AppName}.DAL.Specifications",
                        }.Order())),
                Arg.Any<CancellationToken>());
    }
}
