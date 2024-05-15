using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.Models;
using System.IO.Compression;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class ConfigurationsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ICaseTransformer _caseTransformer;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public ConfigurationsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _caseTransformer = Substitute.For<ICaseTransformer>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new ConfigurationsGenCommand(_fileGenService, _caseTransformer);
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
                        new() { Name = "Professor", Type = DotnetTypes.Int, IsNavigation = true, Relation = new DotnetEntityRelationModel { OnDeleteAction = "CASCADE", ForeignPropertyName = "professor_id" } },
                    }
                }
            })
            .Create();

        var configurationModel = new EntityConfigurationModel
        {
            Namespace = $"CoolApp.DAL.Configurations",
            ConfigurationName = $"StudentConfiguration",
            EntityName = $"StudentEntity",
            TableName = "student",
            IdColumnName = "student_id",
            HasPK = true,
            Usings = new[]
            {
                "CoolApp.DAL.Entities",
                "Microsoft.EntityFrameworkCore",
                "Microsoft.EntityFrameworkCore.Metadata.Builders",
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityConfigurationModel>(x =>
                    x.Namespace == configurationModel.Namespace &&
                    x.ConfigurationName == configurationModel.ConfigurationName &&
                    x.EntityName == configurationModel.EntityName &&
                    x.TableName == configurationModel.TableName &&
                    x.IdColumnName == configurationModel.IdColumnName &&
                    x.HasPK == configurationModel.HasPK &&
                    x.Usings.SequenceEqual(configurationModel.Usings) &&
                    x.ColumnConfigs.Count == 1 &&
                    x.RelationConfigs.Count == 1),
                Arg.Any<CancellationToken>());
    }
}
