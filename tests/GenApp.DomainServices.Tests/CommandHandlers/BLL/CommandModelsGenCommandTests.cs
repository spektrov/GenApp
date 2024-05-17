using AutoMapper;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.BLL;
using GenApp.DomainServices.Mappers;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.BLL;
public class CommandModelsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly IMapper _mapper;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public CommandModelsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TemplatesMapperProfile>();
        }).CreateMapper();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new CommandModelsGenCommand(_fileGenService, _mapper);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters()
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
                        new() { Name = "CourseId", Type = DotnetTypes.Int, IsForeignRelation = true, },
                        new() { Name = "Course", Type = DotnetTypes.Int, IsNavigation = true, },
                    }
                }
            })
            .Create();

        var commmandModel = new CommandModelModel
        {
            Namespace = $"{model.AppName}.BLL.CommandModels",
            ModelName = $"StudentCommandModel",
            Properties = new List<DotnetPropertyDto>(),
            Usings = Array.Empty<string>(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("CommandModel.cs")),
                Arg.Is<CommandModelModel>(x =>
                    x.Namespace == commmandModel.Namespace &&
                    x.ModelName == commmandModel.ModelName &&
                    x.Usings!.SequenceEqual(commmandModel.Usings) &&
                    x.Properties.Count == 3),
                Arg.Any<CancellationToken>());
    }
}
