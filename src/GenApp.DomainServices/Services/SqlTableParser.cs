using System.Text.RegularExpressions;
using FluentResults;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;

namespace GenApp.DomainServices.Services;
internal class SqlTableParser : ISqlTableParser
{
    private static readonly string TablePropertyPattern = @"\,\d+|[\)\(;]|\b\d+\b|[\n]";
    private static readonly char ComaSeparator = ',';
    private static readonly char SpaceSeparator = ' ';
    private static readonly string NotNull = "not null";
    private static readonly string Unique = "unique";
    private static readonly string PrimaryKey = "primary key";

    public Result<SqlTableConfigurationModel> BuildTableConfiguration(string tableLine)
    {
        var tableName = tableLine.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();
        var allItems = Regex.Replace(tableLine, TablePropertyPattern, string.Empty)[tableName.Length..];

        var items = allItems.Split(ComaSeparator, StringSplitOptions.TrimEntries);
        var aItems = items.Where(x => !string.IsNullOrWhiteSpace(x));
        var columns = aItems.Select(BuildColumnConfiguration);

        if (columns.Any(x => x.IsFailed))
        {
            return Result.Fail(columns.Where(x => x.IsFailed).SelectMany(x => x.Errors));
        }

        return new SqlTableConfigurationModel
        {
            TableName = tableName,
            Columns = columns.Where(x => x.IsSuccess).Select(x => x.Value),
        };
    }

    private Result<SqlColumnConfigurationModel> BuildColumnConfiguration(string columnLine)
    {
        // Split column line into components and remove leading/trailing white space from each component
        var columnComponents = columnLine.Split(SpaceSeparator)
            .Select(component => component.Trim()).ToList();

        if (columnComponents.Count < 2)
        {
            return Result.Fail($"Cannot define name, type, and constraints on column\n{columnLine}");
        }

        var column = new SqlColumnConfigurationModel
        {
            ColumnName = columnComponents[0], // First component should be column name
            ColumnType = columnComponents[1], // Second component should be column type
            NotNull = DefineIfNotNull(columnLine),
            IsPrimaryKey = DefineIfPrimaryKey(columnLine),
        };

        return column;
    }

    private bool DefineIfPrimaryKey(string columnDefinition)
    {
        return columnDefinition.Contains(PrimaryKey, StringComparison.OrdinalIgnoreCase);
    }

    private bool DefineIfNotNull(string columnDefinition)
    {
        var isNotNull = columnDefinition.Contains(NotNull, StringComparison.OrdinalIgnoreCase)
            || columnDefinition.Contains(Unique, StringComparison.OrdinalIgnoreCase)
            || DefineIfPrimaryKey(columnDefinition);

        return isNotNull;
    }
}
