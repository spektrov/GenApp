using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Mappers;
using GenApp.Parsers.Abstractions.Constants;
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
            SqlTableScript = "create table person (person_id INT PRIMARY KEY)",
            DbmsType = DbmsType.POSTGRESQL,
            DotnetSdkVersion = 8,
            UseDocker = true,
        };

        var result = _sut.Map(appData);

        result.IsSuccess.Should().BeTrue();
        result.Value.AppName.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_should_set_one_entity_with_primary_key_for_postgresql()
    {
        var appData = new ApplicationDataModel
        {
            AppName = "App",
            SqlTableScript = "create table person (person_id SERIAL PRIMARY KEY,\r\n  name VARCHAR(255) NOT NULL\r\n);",
            DbmsType = DbmsType.POSTGRESQL,
            DotnetSdkVersion = 8,
            UseDocker = true,
        };

        var expected = new ApplicationDataModel
        {
            Entities = new List<DotnetEntityConfigurationModel>
            {
                new()
                {
                    EntityName = "Person",
                    HasId = true,
                    IdType = DotnetTypes.Int,
                    Table = new() { Name = "person", KeyName = "person_id", },
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new()
                        {
                            Name = "Id",
                            Type = DotnetTypes.Int,
                            ColumnName = "person_id",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = true,
                            NotNull = true,
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
                            NotNull = true,
                            Relation = null,
                        },
                    }
                },
            }
        };

        var result = _sut.Map(appData);

        result.IsSuccess.Should().BeTrue();
        result.Value.Entities.Should().BeEquivalentTo(expected.Entities);
    }

    [Fact]
    public void Map_should_set_one_entity_with_primary_key_for_mysql()
    {
        var appData = new ApplicationDataModel
        {
            AppName = "App",
            SqlTableScript = "create table person (person_id INT PRIMARY KEY, name varchar(255) NOT NULL)",
            DbmsType = DbmsType.MYSQL,
            DotnetSdkVersion = 8,
            UseDocker = true,
        };

        var expected = new ApplicationDataModel
        {
            Entities = new List<DotnetEntityConfigurationModel>
            {
                new()
                {
                    EntityName = "Person",
                    HasId = true,
                    IdType = DotnetTypes.Int,
                    Table = new() { Name = "person", KeyName = "person_id", },
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new()
                        {
                            Name = "Id",
                            Type = DotnetTypes.Int,
                            ColumnName = "person_id",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = true,
                            NotNull = true,
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
                            NotNull = true,
                            Relation = null,
                        },
                    }
                },
            }
        };

        var result = _sut.Map(appData);

        result.IsSuccess.Should().BeTrue();
        result.Value.Entities.Should().BeEquivalentTo(expected.Entities);
    }

    [Fact]
    public void Map_should_set_one_entity_with_primary_key_for_sql_server()
    {
        var appData = new ApplicationDataModel
        {
            AppName = "App",
            SqlTableScript = "create table person (person_id INT PRIMARY KEY, name NVARCHAR(255) NOT NULL)",
            DbmsType = DbmsType.MSSQLSERVER,
            DotnetSdkVersion = 8,
            UseDocker = true,
        };

        var expected = new ApplicationDataModel
        {
            Entities = new List<DotnetEntityConfigurationModel>
            {
                new()
                {
                    EntityName = "Person",
                    HasId = true,
                    IdType = DotnetTypes.Int,
                    Table = new() { Name = "person", KeyName = "person_id", },
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new()
                        {
                            Name = "Id",
                            Type = DotnetTypes.Int,
                            ColumnName = "person_id",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = true,
                            NotNull = true,
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
                            NotNull = true,
                            Relation = null,
                        },
                    }
                },
            }
        };

        var result = _sut.Map(appData);

        result.IsSuccess.Should().BeTrue();
        result.Value.Entities.Should().BeEquivalentTo(expected.Entities);
    }

    [Fact]
    public void Map_should_set_entity_with_relation_entity_for_postgresql()
    {
        var appData = new ApplicationDataModel
        {
            AppName = "App",
            SqlTableScript = "create table person (person_id integer PRIMARY KEY, name varchar(255) NOT NULL);\n" +
            "create table profile (profile_id integer PRIMARY KEY, person_id integer NOT NULL, completed bool, FOREIGN KEY (person_id) REFERENCES person (person_id) on delete cascade,\r\n);",
            DbmsType = DbmsType.POSTGRESQL,
            DotnetSdkVersion = 8,
            UseDocker = true,
        };

        var expected = new ApplicationDataModel
        {
            Entities = new List<DotnetEntityConfigurationModel>
            {
                new()
                {
                    EntityName = "Person",
                    HasId = true,
                    IdType = DotnetTypes.Int,
                    Table = new() { Name = "person", KeyName = "person_id", },
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new()
                        {
                            Name = "Id",
                            Type = DotnetTypes.Int,
                            ColumnName = "person_id",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = true,
                            NotNull = true,
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
                            NotNull = true,
                            Relation = null,
                        },
                        new()
                        {
                            Name = "Profile",
                            Type = "Profile",
                            IsForeignRelation = false,
                            IsNavigation = true,
                            IsId = false,
                            NotNull = false,
                            Relation = new DotnetEntityRelationModel
                            {
                                SourceEntity = "Person",
                                TargetEntity = "Profile",
                                IsOneToOne = false,
                                IsRequired = false,
                                IsReverted = true,
                            },
                        },
                    }
                },
                new()
                {
                    EntityName = "Profile",
                    HasId = true,
                    IdType = DotnetTypes.Int,
                    Table = new() { Name = "profile", KeyName = "profile_id", },
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new()
                        {
                            Name = "Id",
                            Type = DotnetTypes.Int,
                            ColumnName = "profile_id",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = true,
                            NotNull = true,
                            Relation = null,
                        },
                        new()
                        {
                            Name = "PersonId",
                            Type = DotnetTypes.Int,
                            ColumnName = "person_id",
                            IsForeignRelation = true,
                            IsNavigation = false,
                            IsId = false,
                            NotNull = true,
                            Relation = null,
                        },
                        new()
                        {
                            Name = "Person",
                            Type = "Person",
                            IsForeignRelation = false,
                            IsNavigation = true,
                            IsId = false,
                            NotNull = true,
                            Relation = new DotnetEntityRelationModel
                            {
                                SourceEntity = "Profile",
                                TargetEntity = "Person",
                                ForeignPropertyName = "PersonId",
                                IsReverted = false,
                                RevertedPropertyName = "Profile",
                                OnDeleteAction = DotnetOnDeleteActions.Cascade,
                                IsOneToOne = false,
                                IsRequired = true,
                            },
                        },
                        new()
                        {
                            Name = "Completed",
                            Type = DotnetTypes.Bool,
                            ColumnName = "completed",
                            IsForeignRelation = false,
                            IsNavigation = false,
                            IsId = false,
                            NotNull = false,
                            Relation = null,
                        },
                    }
                },
            }
        };

        var result = _sut.Map(appData);

        result.IsSuccess.Should().BeTrue();
        result.Value.Entities.Should().BeEquivalentTo(expected.Entities);
    }
}
