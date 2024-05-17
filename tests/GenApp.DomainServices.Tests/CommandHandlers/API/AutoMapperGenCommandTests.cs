using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
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

        var mappingModel = new ApiMappingModelDto
        {
            CommandModelName = $"StudentCommandModel",
            ModelName = $"StudentModel",
            CreateRequestName = $"StudentCreateRequest",
            UpdateRequestName = $"StudentUpdateRequest",
            ResponseName = $"StudentResponse",
        };

        var usings = new List<string>
        {
            "AutoMapper",
            $"{model.AppName}.API.Models.Responses",
            $"{model.AppName}.BLL.CommandModels",
            $"{model.AppName}.BLL.DomainModels",
            $"{model.AppName}.API.Models.Requests.StudentRequests"
        };

        var autoMapperModel = new ApiAutoMapperModel
        {
            Namespace = $"{model.AppName}.API.AutoMapper",
            Models = new List<ApiMappingModelDto>() { mappingModel },
            Usings = usings.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService.Received(1)
        .CreateEntryAsync(
            Arg.Any<ZipArchive>(),
            Arg.Any<string>(),
            Arg.Is<ApiAutoMapperModel>(x =>
                x.Namespace == autoMapperModel.Namespace &&
                x.Models.SequenceEqual(autoMapperModel.Models, new ReflectionObjectEqualityComparer<ApiMappingModelDto>()) &&
                x.Usings.SequenceEqual(autoMapperModel.Usings)),
            Arg.Any<CancellationToken>());
    }
}
