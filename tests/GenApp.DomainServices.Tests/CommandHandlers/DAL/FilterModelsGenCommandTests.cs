using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Services;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class FilterModelsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;
    private readonly ApplicationDataModel _applicationDataModel;

    private readonly IGenCommand _sut;

    public FilterModelsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new FilterModelsGenCommand(_fileGenService, _caseTransformer);

        _applicationDataModel = SetupSingleEntityModel();
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_base_FilterParameters()
    {
        var model = new FilterParametersModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Models",
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("FilterParameters.cs")),
                Arg.Is<FilterParametersModel>(x => x.Namespace == model.Namespace),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_AppJsonSerializerOptions()
    {
        var model = new AppJsonSerializerOptionsModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Models",
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("AppJsonSerializerOptions.cs")),
                Arg.Is<AppJsonSerializerOptionsModel>(x => x.Namespace == model.Namespace),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_base_RangeParameters()
    {
        var model = new RangeParametersModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Models",
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("RangeParameters.cs")),
                Arg.Is<RangeParametersModel>(x => x.Namespace == model.Namespace),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_SearchParameters()
    {
        var model = new EntityFilterParametersModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Models.StudentModels",
            Name = $"StudentSearchParameters",
            Properties = new List<FilterPropertyDto>(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityFilterParametersModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.Name == model.Name &&
                    x.Properties.Count == 1 &&
                    x.Properties.First().FilterName == "Name"),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_RangeParameters()
    {
        var model = new EntityFilterParametersModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Models.StudentModels",
            Name = $"StudentRangeParameters",
            Properties = new List<FilterPropertyDto>(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityFilterParametersModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.Name == model.Name &&
                    x.Properties.Count == 1 &&
                    x.Properties.First().FilterName == "Year"),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_FilterParameters()
    {
        var model = new EntityFilterParametersModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Models.StudentModels",
            Name = $"StudentFilterParameters",
            Properties = new List<FilterPropertyDto>(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityFilterParametersModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.Name == model.Name &&
                    x.Properties.Count == 2),
                Arg.Any<CancellationToken>());
    }

    private ApplicationDataModel SetupSingleEntityModel()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MYSQL)
            .With(x => x.Entities, new List<DotnetEntityConfigurationModel>
            {
                new()
                {
                    EntityName = "Student",
                    HasId = true,
                    Table = new SqlTableInfoModel() { Name = "student", KeyName = "student_id" },
                    IdType = DotnetTypes.Int,
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new() { Name = "Id", Type = DotnetTypes.Int, IsId = true },
                        new() { Name = "Name", Type = DotnetTypes.String },
                        new() { Name = "Year", Type = DotnetTypes.Int },
                    }
                },
            })
            .Create();

        return model;
    }
}
