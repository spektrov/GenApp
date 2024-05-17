using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.API;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Services;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.API;
public class ControllerGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public ControllerGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new ControllerGenCommand(_fileGenService, _caseTransformer);
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

        var plural = _caseTransformer.ToPascalCase(_caseTransformer.ToPlural("Student"));

        var controllerModel = new ControllerModel
        {
            Namespace = $"{model.AppName}.API.Controllers",
            ControllerName = $"{plural}Controller",
            ServiceInterfaceName = "IStudentService",
            ResponseModelName = "StudentResponse",
            CreateRequestName = "StudentCreateRequest",
            UpdateRequestName = "StudentUpdateRequest",
            CommandModelName = "StudentCommandModel",
            KeyType = DotnetTypes.Int,
            Usings = new List<string>
        {
            "AutoMapper",
            "Microsoft.AspNetCore.Mvc",
            $"{model.AppName}.API.Models.Requests.StudentRequests",
            $"{model.AppName}.API.Models.Responses",
            $"{model.AppName}.BLL.CommandModels",
            $"{model.AppName}.BLL.Interfaces",
            $"{model.AppName}.BLL.Models",
        }.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService.Received(1)
        .CreateEntryAsync(
            Arg.Any<ZipArchive>(),
            Arg.Any<string>(),
            Arg.Is<ControllerModel>(x =>
                x.Namespace == controllerModel.Namespace &&
                x.ControllerName == controllerModel.ControllerName &&
                x.ServiceInterfaceName == controllerModel.ServiceInterfaceName &&
                x.ResponseModelName == controllerModel.ResponseModelName &&
                x.CreateRequestName == controllerModel.CreateRequestName &&
                x.UpdateRequestName == controllerModel.UpdateRequestName &&
                x.CommandModelName == controllerModel.CommandModelName &&
                x.KeyType == controllerModel.KeyType &&
                x.Usings.SequenceEqual(controllerModel.Usings)),
            Arg.Any<CancellationToken>());
    }
}
