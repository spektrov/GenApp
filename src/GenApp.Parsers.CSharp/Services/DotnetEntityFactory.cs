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
            Properties = table.Columns.Select(column => MapToProperty(column, dbms)).ToList(),
        }).ToList();

        AddNavigationCollections(entities);

        return Result.Ok(entities.AsEnumerable());
    }

    private DotnetPropertyConfigurationModel MapToProperty(SqlColumnConfigurationModel column, DbmsType dbms)
    {
        // TODO: handle when not simple PK - should not create id property
        var property = new DotnetPropertyConfigurationModel
        {
            Name = GetPropertyName(column),
            Type = GetPropertyType(column, dbms),
            NotNull = column.NotNull,
            IsId = column.IsPrimaryKey,
            IsNavigation = column.IsForeignKey,
            Relation = relationMapper.Map(column.Relation),
        };

        return property;
    }

    private string GetPropertyName(SqlColumnConfigurationModel column)
    {
        if (column.IsPrimaryKey)
        {
            return "Id";
        }

        if (column.IsForeignKey && column.Relation is not null &&
            column.ColumnName.Contains("Id", StringComparison.OrdinalIgnoreCase))
        {
            return ToDotnetName(column.Relation.TargetTable);
        }

        return ToDotnetName(column.ColumnName);
    }

    private string GetPropertyType(SqlColumnConfigurationModel column, DbmsType dbms)
    {
        if (column.IsForeignKey && column.Relation is not null)
        {
            return ToDotnetName(column.Relation.TargetTable);
        }

        return PropertyTypeMapper.Map(dbms, column.ColumnType);
    }

    private void AddNavigationCollections(IEnumerable<DotnetEntityConfigurationModel> entities)
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

    private string ToDotnetName(string tableName)
    {
        var singular = caseTransformer.ToSignular(tableName);
        var pascal = caseTransformer.ToPascalCase(singular);

        return pascal;
    }
}
