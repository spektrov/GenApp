﻿using FluentResults;
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
            HasId = table.Columns.Count(x => x.IsPrimaryKey) == 1,
            IdType = GetIdType(table, dbms),
            Table = GetTableInfo(table),
            Properties = table.Columns.SelectMany(column => MapToProperty(column, dbms)).ToList(),
        }).ToList();

        AddRevertedNavigationProperties(entities);

        return Result.Ok(entities.AsEnumerable());
    }

    private IEnumerable<DotnetPropertyConfigurationModel> MapToProperty(SqlColumnConfigurationModel column, DbmsType dbms)
    {
        var properties = new List<DotnetPropertyConfigurationModel>();

        var property = new DotnetPropertyConfigurationModel
        {
            Name = GetPropertyName(column),
            Type = GetPropertyType(column.ColumnType, dbms),
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
                Name = GetNavigationPropertyName(column),
                Type = ToDotnetName(column.Relation!.TargetTable),
                NotNull = column.NotNull,
                IsNavigation = true,
                Relation = relationMapper.Map(column.Relation),
            };
            if (navigationPropety.Relation is not null)
            {
                navigationPropety.Relation.ForeignPropertyName = property.Name;
            }

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

    private string GetNavigationPropertyName(SqlColumnConfigurationModel column)
    {
        if (!column.IsForeignKey || column.Relation is null)
        {
            return ToDotnetName(column.ColumnName);
        }

        if (column.Relation.HasManyFKToOneTable)
        {
            return ToDotnetName(RemoveIdSuffix(column.ColumnName));
        }

        return ToDotnetName(column.Relation.TargetTable);
    }

    private string GetRevertedNavigationPropertyName(
        string sourceEntityName, string targetEntityName, string navigationPropertyName)
    {
        return navigationPropertyName == targetEntityName
            ? sourceEntityName
            : navigationPropertyName + sourceEntityName;
    }

    private string RemoveIdSuffix(string input)
    {
        var result = input.EndsWith("id", StringComparison.OrdinalIgnoreCase) ? input[..^2] : input;
        result = input.EndsWith("_id", StringComparison.OrdinalIgnoreCase) ? input[..^3] : result;
        return result;
    }

    private string GetPropertyType(string columnType, DbmsType dbms)
    {
        return PropertyTypeMapper.Map(dbms, columnType);
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
                            Name = GetRevertedNavigationPropertyName(entity.EntityName, targetEntityName, property.Name),
                            Type = entity.EntityName,
                            IsNavigation = true,
                            Relation = relationMapper.MapReverted(property.Relation),
                        };
                        targetEntity.Properties.Add(navigationProperty);
                        property.Relation.RevertedPropertyName = navigationProperty.Name;
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
        }

        return tableInfo;
    }

    private string? GetIdType(SqlTableConfigurationModel table, DbmsType dbms)
    {
        var idType = table.Columns.FirstOrDefault(x => x.IsPrimaryKey)?.ColumnType;
        return !string.IsNullOrEmpty(idType)
            ? GetPropertyType(idType, dbms)
            : default;
    }

    private string ToDotnetName(string tableName)
    {
        var singular = caseTransformer.ToSignular(tableName);
        var pascal = caseTransformer.ToPascalCase(singular);

        return pascal;
    }
}
