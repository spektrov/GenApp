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
public class DatabaseContextGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public DatabaseContextGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new DatabaseContextGenCommand(_fileGenService, _caseTransformer);
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
                    Table = new SqlTableInfoModel() { Name = "student", KeyName = "student_id" },
                    IdType = DotnetTypes.Int,
                }
            })
            .Create();

        var contextModel = new DatabaseContextModel
        {
            Namespace = $"{model.AppName}.DAL",
            Usings = new[]
                {
                    "Microsoft.EntityFrameworkCore",
                    $"{model.AppName}.DAL.Entities",
                }.Order(),
            Sets = new List<DbSetDto>
            {
                new() { EntityName = "StudentEntity", CollectionName = "Students" },
                new() { EntityName = "ProfessorEntity", CollectionName = "Professors" },
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("DatabaseContext.cs")),
                Arg.Is<DatabaseContextModel>(x =>
                    x.Namespace == contextModel.Namespace &&
                    x.Usings.SequenceEqual(contextModel.Usings) &&
                    x.Sets.SequenceEqual(contextModel.Sets, new ReflectionObjectEqualityComparer<DbSetDto>())),
                Arg.Any<CancellationToken>());
    }
}
