using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.BLL;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Services;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.BLL;
public class ServicesGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public ServicesGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new ServicesGenCommand(_fileGenService, _caseTransformer);
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
                        new() { Name = "Course", Type = "Course", IsNavigation = true, Relation = new() { TargetEntity = "Course" } },
                    }
                },
                new()
                {
                    EntityName = "Course",
                    HasId = true,
                    Table = new SqlTableInfoModel() { Name = "course", KeyName = "course_id" },
                    IdType = DotnetTypes.Int,
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new() { Name = "Id", Type = DotnetTypes.Int, IsId = true },
                        new() { Name = "Name", Type = DotnetTypes.String },
                    }
                }
            })
            .Create();

        var serviceModel = new ServiceModel
        {
            Namespace = $"{model.AppName}.BLL.Services",
            ServiceName = "StudentService",
            InterfaceName = "IStudentService",
            RepositoryInterfaceName = "IStudentRepository",
            ModelName = "StudentModel",
            EntityName = "StudentEntity",
            CommandModelName = "StudentCommandModel",
            FindByIdSpecification = "FindStudentById",
            KeyType = DotnetTypes.Int,
            GetManyIncludes = new List<string>() { "StudentEntity.Course" },
            GetByIdIncludes = new List<string> { "StudentEntity.Course" },
            PropertiesForUpdate = new List<string> { "Name", "CourseId" },
            Usings = new List<string>
            {
                "AutoMapper",
                "Microsoft.Extensions.Logging",
                $"{model.AppName}.BLL.CommandModels",
                $"{model.AppName}.BLL.DomainModels",
                $"{model.AppName}.BLL.Exceptions",
                $"{model.AppName}.BLL.Interfaces",
                $"{model.AppName}.BLL.Models",
                $"{model.AppName}.DAL.Entities",
                $"{model.AppName}.DAL.Helpers",
                $"{model.AppName}.DAL.Interfaces",
                $"{model.AppName}.DAL.Models",
                $"{model.AppName}.DAL.Specifications.StudentSpecifications"
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService.Received(1).CreateEntryAsync(
            Arg.Any<ZipArchive>(),
            Arg.Any<string>(),
            Arg.Is<ServiceModel>(x =>
                x.Namespace == serviceModel.Namespace &&
                x.ServiceName == serviceModel.ServiceName &&
                x.InterfaceName == serviceModel.InterfaceName &&
                x.RepositoryInterfaceName == serviceModel.RepositoryInterfaceName &&
                x.ModelName == serviceModel.ModelName &&
                x.EntityName == serviceModel.EntityName &&
                x.CommandModelName == serviceModel.CommandModelName &&
                x.FindByIdSpecification == serviceModel.FindByIdSpecification &&
                x.KeyType == serviceModel.KeyType &&
                x.GetManyIncludes.SequenceEqual(serviceModel.GetManyIncludes) &&
                x.GetByIdIncludes.SequenceEqual(serviceModel.GetByIdIncludes) &&
                x.PropertiesForUpdate.SequenceEqual(serviceModel.PropertiesForUpdate) &&
                x.Usings!.SequenceEqual(serviceModel.Usings)),
            Arg.Any<CancellationToken>());
    }
}
