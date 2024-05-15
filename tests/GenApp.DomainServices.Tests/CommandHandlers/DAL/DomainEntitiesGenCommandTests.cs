using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.DomainServices.Mappers;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Services;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class DomainEntitiesGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly IMapper _mapper;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public DomainEntitiesGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TemplatesMapperProfile>();
        }).CreateMapper();

        _sut = new DomainEntitiesGenCommand(_fileGenService, _mapper, _caseTransformer);
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
                },
                new()
                {
                    EntityName = "Professor",
                    HasId = true,
                    Table = new SqlTableInfoModel() { Name = "professor", KeyName = "professor_id" },
                    IdType = DotnetTypes.Int,
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new() { Name = "Id", Type = DotnetTypes.Int, IsId = true },
                        new() { Name = "Name", Type = DotnetTypes.String },
                    }
                }
            })
            .Create();

        var studentModel = new DomainEntityModel
        {
            Namespace = $"{model.AppName}.DAL.Entities",
            Usings = new[]
            {
                $"{model.AppName}.DAL.Interfaces"
            },
            EntityName = "StudentEntity",
            HasId= true,
            KeyType = DotnetTypes.Int,
            Properties = new List<DotnetPropertyDto>(),
        };

        var professorModel = new DomainEntityModel
        {
            Namespace = $"{model.AppName}.DAL.Entities",
            Usings = new[]
            {
                $"{model.AppName}.DAL.Interfaces"
            },
            EntityName = "ProfessorEntity",
            HasId = true,
            KeyType = DotnetTypes.Int,
            Properties = new List<DotnetPropertyDto>(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("StudentEntity.cs")),
                Arg.Is<DomainEntityModel>(x =>
                    x.Namespace == studentModel.Namespace &&
                    x.EntityName == studentModel.EntityName &&
                    x.Usings.SequenceEqual(studentModel.Usings) &&
                    x.KeyType == studentModel.KeyType),
                Arg.Any<CancellationToken>());

        await _fileGenService
           .Received(1)
           .CreateEntryAsync(
               Arg.Any<ZipArchive>(),
               Arg.Is<string>(x => x.Contains("ProfessorEntity.cs")),
               Arg.Is<DomainEntityModel>(x =>
                   x.Namespace == professorModel.Namespace &&
                   x.EntityName == professorModel.EntityName &&
                   x.Usings.SequenceEqual(professorModel.Usings) &&
                   x.KeyType == professorModel.KeyType),
               Arg.Any<CancellationToken>());
    }
}
