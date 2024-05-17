using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Services;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class RepositoryInterfacesGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public RepositoryInterfacesGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new RepositoryInterfacesGenCommand(_fileGenService, _caseTransformer);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_BaseRepositoryInterface()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.AppName, "CoolApp")
            .Create();

        var interfaceModel = new RepositoryBaseInterfaceModel
        {
            Namespace = $"CoolApp.DAL.Interfaces",
            Usings = new[]
            {
                "CoolApp.DAL.Enums",
                "CoolApp.DAL.Models",
                "CoolApp.DAL.Specifications",
                "Microsoft.EntityFrameworkCore",
                "System.Linq.Expressions",
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("IRepositoryBase.cs")),
                Arg.Is<RepositoryBaseInterfaceModel>(x =>
                    x.Namespace == interfaceModel.Namespace &&
                    x.Usings.SequenceEqual(interfaceModel.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_EntityRepositoryInterfaces()
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
                    IdType = DotnetTypes.Guid,
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new() { Name = "Id", Type = DotnetTypes.Guid, IsId = true },
                        new() { Name = "Name", Type = DotnetTypes.String },
                    }
                }
            })
            .Create();

        var studentInterface = new RepositoryInterfaceModel
        {
            Name = $"IStudentRepository",
            EntityName = "StudentEntity",
            KeyType = DotnetTypes.Int,
            Namespace = $"{model.AppName}.DAL.Interfaces",
            Usings = new[] { $"{model.AppName}.DAL.Entities" },
        };

        var professorInterface = new RepositoryInterfaceModel
        {
            Name = $"IProfessorRepository",
            EntityName = "ProfessorEntity",
            KeyType = DotnetTypes.Guid,
            Namespace = $"{model.AppName}.DAL.Interfaces",
            Usings = new[] { $"{model.AppName}.DAL.Entities" },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
           .Received(1)
           .CreateEntryAsync(
               Arg.Any<ZipArchive>(),
               Arg.Any<string>(),
               Arg.Is<RepositoryInterfaceModel>(x =>
                   x.Namespace == studentInterface.Namespace &&
                   x.Name == studentInterface.Name &&
                   x.EntityName == studentInterface.EntityName &&
                   x.Usings.SequenceEqual(studentInterface.Usings) &&
                   x.KeyType == studentInterface.KeyType),
               Arg.Any<CancellationToken>());

        await _fileGenService
          .Received(1)
          .CreateEntryAsync(
              Arg.Any<ZipArchive>(),
              Arg.Any<string>(),
              Arg.Is<RepositoryInterfaceModel>(x =>
                  x.Namespace == professorInterface.Namespace &&
                  x.Name == professorInterface.Name &&
                  x.EntityName == professorInterface.EntityName &&
                  x.Usings.SequenceEqual(professorInterface.Usings) &&
                  x.KeyType == professorInterface.KeyType),
              Arg.Any<CancellationToken>());
    }
}
