using FluentResults;
using GenApp.Domain.Enums;
using GenApp.Domain.Models;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.Abstractions.Models;
using GenApp.Parsers.CSharp.Interfaces;
using GenApp.Parsers.CSharp.Mappers;

namespace GenApp.Parsers.CSharp.Services;
internal class DotnetEntityFactory(ICaseTransformer caseTransformer, IDotnetRelationMapper relationMapper) : IDotnetEntityFactory
{
    public Result<IEnumerable<DotnetEntityConfigurationModel>> Create(
        IEnumerable<SqlTableConfigurationModel> tables, DbmsType dbms)
    {
        var entities = tables.Select(table => new DotnetEntityConfigurationModel
        {
            EntityName = ToDotnetName(table.TableName),
            Table = GetTableInfo(table),
            Properties = table.Columns.SelectMany(column => MapToProperty(column, dbms)).ToList(),
        }).ToList();

        AddRevertedNavigationProperties(entities);

        return Result.Ok(entities.AsEnumerable());
    }

    private IEnumerable<DotnetPropertyConfigurationModel> MapToProperty(SqlColumnConfigurationModel column, DbmsType dbms)
    {
        // TODO: handle when not simple PK - should not create id property
        // TODO: handle when FK - should create navigation and FK id property
        var properties = new List<DotnetPropertyConfigurationModel>();

        var property = new DotnetPropertyConfigurationModel
        {
            Name = GetPropertyName(column),
            Type = GetPropertyType(column, dbms),
            NotNull = column.NotNull,
            IsId = column.IsPrimaryKey,
            IsForeignRelation = column.IsForeignKey,
            ColumnName = column.ColumnName,
        };

        properties.Add(property);

        if (column.IsForeignKey && column.Relation is not null)
        {
            var navigationPropety = new DotnetPropertyConfigurationModel
            {
                Name = ToDotnetName(column.Relation!.TargetTable),
                Type = column.Relation.TargetTable,
                NotNull = column.NotNull,
                IsNavigation = true,
                Relation = relationMapper.Map(column.Relation),
            };

            properties.Add(navigationPropety);
        }

        return properties;
    }

    private string GetPropertyName(SqlColumnConfigurationModel column)
    {
        if (column.IsPrimaryKey)
        {
            return "Id";
        }

        return ToDotnetName(column.ColumnName);
    }

    private string GetPropertyType(SqlColumnConfigurationModel column, DbmsType dbms)
    {
        return PropertyTypeMapper.Map(dbms, column.ColumnType);
    }

    private void AddRevertedNavigationProperties(IEnumerable<DotnetEntityConfigurationModel> entities)
    {
        var entityLookup = entities.ToDictionary(e => e.EntityName, e => e);
        foreach (var entity in entities)
        {
            foreach (var property in entity.Properties)
            {
                if (property.IsNavigation && property.Relation != null)
                {
                    var targetEntityName = ToDotnetName(property.Relation.TargetEntity);
                    if (entityLookup.TryGetValue(targetEntityName, out var targetEntity))
                    {
                        var navigationProperty = new DotnetPropertyConfigurationModel
                        {
                            // TODO: handle when many FK to one table
                            Name = entity.EntityName,
                            Type = entity.EntityName,
                            IsNavigation = true,
                            Relation = relationMapper.MapReverted(property.Relation),
                        };
                        targetEntity.Properties.Add(navigationProperty);
                    }
                }
            }
        }
    }

    private SqlTableInfoModel GetTableInfo(SqlTableConfigurationModel table)
    {
        var tableInfo = new SqlTableInfoModel
        {
            Name = table.TableName,
        };

        var primaryKeyCount = table.Columns.Count(x => x.IsPrimaryKey);
        if (primaryKeyCount == 1)
        {
            var primaryKey = table.Columns.First(x => x.IsPrimaryKey);
            tableInfo.KeyName = primaryKey.ColumnName;
            tableInfo.KeyType = primaryKey.ColumnType;
        }

        return tableInfo;
    }

    private string ToDotnetName(string tableName)
    {
        var singular = caseTransformer.ToSignular(tableName);
        var pascal = caseTransformer.ToPascalCase(singular);

        return pascal;
    }
}
