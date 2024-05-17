using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.DomainServices.Mappers;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Services;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
public class ModelsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly IMapper _mapper;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;
    private readonly ApplicationDataModel _applicationDataModel;

    private readonly IGenCommand _sut;

    public ModelsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TemplatesMapperProfile>();
        }).CreateMapper();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();
        _applicationDataModel = SetupSingleEntityModel();

        _sut = new ModelsGenCommand(_fileGenService, _mapper, _caseTransformer);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_CreateRequest()
    {
        var model = new CommandModelModel
        {
            Namespace = $"{_applicationDataModel.AppName}.API.Models.Requests.StudentRequests",
            ModelName = $"StudentCreateRequest",
            Properties = new List<DotnetPropertyDto>(),
            Usings = Array.Empty<string>(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("CreateRequest.cs")),
                Arg.Is<CommandModelModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.ModelName == model.ModelName &&
                    x.Usings.SequenceEqual(model.Usings) &&
                    x.Properties.Count == 2),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_UpdateRequest()
    {
        var model = new CommandModelModel
        {
            Namespace = $"{_applicationDataModel.AppName}.API.Models.Requests.StudentRequests",
            ModelName = $"StudentUpdateRequest",
            Properties = new List<DotnetPropertyDto>(),
            Usings = Array.Empty<string>(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("UpdateRequest.cs")),
                Arg.Is<CommandModelModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.ModelName == model.ModelName &&
                    x.Usings.SequenceEqual(model.Usings) &&
                    x.Properties.Count == 3),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_Response()
    {
        var model = new DomainModelModel
        {
            Namespace = $"{_applicationDataModel.AppName}.API.Models.Responses",
            ModelName = $"StudentResponse",
            Properties = new List<DotnetPropertyDto>(),
            Usings = Array.Empty<string>(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("Response.cs")),
                Arg.Is<DomainModelModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.ModelName == model.ModelName &&
                    x.Usings.SequenceEqual(model.Usings) &&
                    x.Properties.Count == 3),
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
