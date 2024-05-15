using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Mappers;
using GenApp.Parsers.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GenApp.IntegrationTests;
public class ApplicationDataModelMapperTests
{
    private readonly IApplicationDataMapper _sut;

    public ApplicationDataModelMapperTests()
    {
        var host = new TestHost();
        var sqlTableParser = host.ServiceProvider.GetRequiredService<ISqlTableParser>();
        var caseTransformer = host.ServiceProvider.GetRequiredService<ICaseTransformer>();
        var dotnetEntityFactory = host.ServiceProvider.GetRequiredService<IDotnetEntityFactory>();

        _sut = new ApplicationDataModelMapper(sqlTableParser, caseTransformer, dotnetEntityFactory);
    }

    [Fact]
    public void Map_should_set_app_name()
    {
        var expected = "NewApp";
        var appData = new ApplicationDataModel
        {
            AppName = "new_app",
            SqlTableScript = "create table person (student_id INT PRIMARY KEY)",
            DbmsType = DbmsType.POSTGRESQL,
            DotnetSdkVersion = 8,
            UseDocker = true,
        };

        var result = _sut.Map(appData);

        result.IsSuccess.Should().BeTrue();
        result.Value.AppName.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_should_set_one_entity_with_primary_key()
    {
        var appData = new ApplicationDataModel
        {
            AppName = "App",
            SqlTableScript = "create table person (student_id INT PRIMARY KEY, name NVARCHAR(255) NOT NULL)",
            DbmsType = DbmsType.POSTGRESQL,
            DotnetSdkVersion = 8,
            UseDocker = true,
        };

        var expected = appData;
        expected.Entities = new List<DotnetEntityConfigurationModel>
            {
                new()
                {
                    EntityName = "Person",
                    HasId = true,
                    IdType = DotnetTypes.Int,
                    Table = new() { Name = "person", KeyName = "student_id", },
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new()
                        {
                            Name = "Id",
                            Type = DotnetTypes.Int,
                            ColumnName = "student_id",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = true,
                            NotNull = false,
                            Relation = null,
                        },
                        new()
                        {
                            Name = "Name",
                            Type = DotnetTypes.String,
                            ColumnName = "name",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = false,
                            NotNull = false,
                            Relation = null,
                        },
                    }
                },
            };

        var result = _sut.Map(appData);

        result.IsSuccess.Should().BeTrue();
        result.Value.Entities.Should().BeEquivalentTo(expected.Entities);
    }
}
