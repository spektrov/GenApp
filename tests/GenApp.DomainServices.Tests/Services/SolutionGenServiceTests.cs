using FluentResults;
using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Services;

namespace GenApp.DomainServices.Tests.Services;
public class SolutionGenServiceTests
{
    private readonly IApplicationDataMapper _mockMapper = Substitute.For<IApplicationDataMapper>();
    private readonly IArchiveGenService _mockArchiveGenService = Substitute.For<IArchiveGenService>();
    private readonly IList<IGenCommand> _mockCommands = new List<IGenCommand> { Substitute.For<IGenCommand>() };

    private readonly ISolutionGenService _sut;

    public SolutionGenServiceTests()
    {
        _sut = new SolutionGenService(_mockMapper, _mockArchiveGenService, _mockCommands);
    }

    [Fact]
    public async Task GenerateApplicationAsync_ShouldExecuteCommands_WhenMapperSucceeds()
    {
        var model = new ApplicationDataModel();
        var mappedModel = model;
        _mockMapper.Map(model).Returns(Result.Ok(mappedModel));

        await _sut.GenerateApplicationAsync(model, CancellationToken.None);

        await _mockCommands[0].Received().ExecuteAsync(Arg.Any<ZipArchive>(), mappedModel, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GenerateApplicationAsync_reset_position()
    {
        var model = new ApplicationDataModel();
        var mappedModel = model;
        _mockMapper.Map(model).Returns(Result.Ok(mappedModel));

        await _sut.GenerateApplicationAsync(model, CancellationToken.None);

        _mockArchiveGenService.Received().ResetPosition();
    }
}
