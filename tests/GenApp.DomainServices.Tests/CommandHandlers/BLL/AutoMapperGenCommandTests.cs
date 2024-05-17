using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.BLL;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.BLL;
public class AutoMapperGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public AutoMapperGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new AutoMapperGenCommand(_fileGenService);
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
                    }
                }
            })
            .Create();

        var mapperModel = new BllAutoMapperModel
        {
            Namespace = $"{model.AppName}.BLL.AutoMapper",
            Usings = new[]
                {
                    "AutoMapper",
                    $"{model.AppName}.BLL.CommandModels",
                    $"{model.AppName}.BLL.DomainModels",
                    $"{model.AppName}.DAL.Entities",
                }.Order(),
            Models = new List<BllMappingModelDto>
            {
                new()
                {
                    CommandModelName = $"StudentCommandModel",
                    EntityName = $"StudentEntity",
                    ModelName = $"StudentModel",
                },
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("AutoMapper/Mapper.cs")),
                Arg.Is<BllAutoMapperModel>(x =>
                    x.Namespace == mapperModel.Namespace &&
                    x.Usings!.SequenceEqual(mapperModel.Usings) &&
                    x.Models.SequenceEqual(mapperModel.Models, new ReflectionObjectEqualityComparer<BllMappingModelDto>())),
                Arg.Any<CancellationToken>());
    }
}
