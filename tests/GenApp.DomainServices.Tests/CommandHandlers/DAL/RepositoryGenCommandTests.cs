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
public class RepositoryGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;
    private readonly ApplicationDataModel _applicationDataModel;

    private readonly IGenCommand _sut;

    public RepositoryGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = new CaseTransformer();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();
        _applicationDataModel = SetupSingleEntityModel();

        _sut = new RepositoryGenCommand(_fileGenService, _caseTransformer);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_BaseRepository()
    {
        var repoModel = new RepositoryBaseModel
        {
            Namespace = $"CoolApp.DAL.Repositories",
            Usings = new[]
            {
                "CoolApp.DAL.Enums",
                "CoolApp.DAL.Extensions",
                "CoolApp.DAL.Interfaces",
                "CoolApp.DAL.Models",
                "CoolApp.DAL.Specifications",
                "CoolApp.DAL.Specifications.Orerators",
                "Microsoft.EntityFrameworkCore",
                "System.Linq.Expressions",
                "System.Text.Json",
            },
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("RepositoryBase.cs")),
                Arg.Is<RepositoryBaseModel>(x =>
                    x.Namespace == repoModel.Namespace &&
                    x.Usings.SequenceEqual(repoModel.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_EntityRepository()
    {
        var repoModel = new EntityRepositoryModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Repositories",
            RepositoryName = $"StudentRepository",
            InterfaceName = $"IStudentRepository",
            EntityName = $"StudentEntity",
            KeyType = DotnetTypes.Int,
            FilterParametersName = "StudentFilterParameters",
            SearchParametersName = "StudentSearchParameters",
            RangeParametersName = "StudentRangeParameters",
            FilterParameters = new List<RepositoryFilterDto>(),
            SearchParameters = new List<RepositoryFilterDto>(),
            RangeParameters = new List<RepositoryFilterDto>(),
            SortParameters = new List<SortPropertyDto>(),
            Usings = new[]
            {
                "System.Linq.Expressions",
                "System.Text.Json",
                $"{_applicationDataModel.AppName}.DAL.Entities",
                $"{_applicationDataModel.AppName}.DAL.Interfaces",
                $"{_applicationDataModel.AppName}.DAL.Models",
                $"{_applicationDataModel.AppName}.DAL.Specifications",
                $"{_applicationDataModel.AppName}.DAL.Models.StudentModels",
                $"{_applicationDataModel.AppName}.DAL.Specifications.StudentSpecifications",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityRepositoryModel>(x =>
                    x.Namespace == repoModel.Namespace &&
                    x.RepositoryName == repoModel.RepositoryName &&
                    x.InterfaceName == repoModel.InterfaceName &&
                    x.EntityName == repoModel.EntityName &&
                    x.KeyType == repoModel.KeyType &&
                    x.FilterParametersName == repoModel.FilterParametersName &&
                    x.SearchParametersName == repoModel.SearchParametersName &&
                    x.RangeParametersName == repoModel.RangeParametersName &&
                    x.FilterParameters.Count == 2 &&
                    x.SearchParameters.Count == 1 &&
                    x.RangeParameters.Count == 1 &&
                    x.SortParameters.Count == 2 &&
                    x.Usings.SequenceEqual(repoModel.Usings)),
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
