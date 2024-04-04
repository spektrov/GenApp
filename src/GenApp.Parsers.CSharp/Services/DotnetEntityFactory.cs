using FluentResults;
using GenApp.Domain.Enums;
using GenApp.Domain.Models;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.Abstractions.Models;

namespace GenApp.Parsers.CSharp.Services;
internal class DotnetEntityFactory(ICaseTransformer caseTransformer) : IDotnetEntityFactory
{
    public Result<IEnumerable<DotnetEntityConfigurationModel>> Create(
        IEnumerable<SqlTableConfigurationModel> tables, DbmsType dbms)
    {
        var entities = tables.Select(table => new DotnetEntityConfigurationModel
        {
            EntityName = caseTransformer.ToPascalCase(table.TableName),
            Properties = table.Columns
            .Select(column => new DotnetPropertyConfigurationModel
            {
                Name = !column.IsPrimaryKey ? caseTransformer.ToPascalCase(column.ColumnName) : "Id",
                Type = PropertyTypeMapper.Map(dbms, column.ColumnType),
                NotNull = column.NotNull,
                IsId = column.IsPrimaryKey,
            }),
        });

        return Result.Ok(entities);
    }
}
