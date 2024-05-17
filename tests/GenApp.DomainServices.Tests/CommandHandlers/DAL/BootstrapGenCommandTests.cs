using System.IO.Compression;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class BootstrapGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public BootstrapGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new BootstrapGenCommand(_fileGenService);
    }

    [Theory]
    [InlineData(DbmsType.POSTGRESQL, "UseNpgsql")]
    [InlineData(DbmsType.MYSQL, "UseMySql")]
    [InlineData(DbmsType.MSSQLSERVER, "UseSqlServer")]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters(DbmsType dbmsType, string dbmsUsage)
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, dbmsType)
            .With(x=> x.Entities, new List<DotnetEntityConfigurationModel>
            {
                new() { EntityName = "Student", HasId = true },
                new() { EntityName = "Teacher", HasId = true },
                new() { EntityName = "TeacherStudent", HasId = false },
            })
            .Create();

        var bootstrapModel = new DalBootstrapModel
        {
            DbmsUsage = dbmsUsage,
            Injections = new List<InjectionDto>
            {
                new() { ClassName = "StudentRepository", InterfaceName = "IStudentRepository" },
                new() { ClassName = "TeacherRepository", InterfaceName = "ITeacherRepository" },
            },
            Namespace = $"CoolApp.DAL",
            Usings = new[]
            {
                "CoolApp.DAL.Interfaces",
                "CoolApp.DAL.Repositories",
                "Microsoft.EntityFrameworkCore",
                "Microsoft.Extensions.Configuration",
                "Microsoft.Extensions.DependencyInjection",
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<DalBootstrapModel>(x =>
                    x.Namespace == bootstrapModel.Namespace &&
                    x.DbmsUsage == bootstrapModel.DbmsUsage &&
                    x.Usings.SequenceEqual(bootstrapModel.Usings) &&
                    x.Injections.SequenceEqual(bootstrapModel.Injections, new ReflectionObjectEqualityComparer<InjectionDto>())),
                Arg.Any<CancellationToken>());
    }
}
