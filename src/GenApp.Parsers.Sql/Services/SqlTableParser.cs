using System.Text.RegularExpressions;
using FluentResults;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.Abstractions.Models;
using GenApp.Parsers.Sql.Interfaces;

namespace GenApp.Parsers.Sql.Services;
internal class SqlTableParser(ISqlRowParser sqlRowParser) : ISqlTableParser
{
    private static readonly string NotPropertyPattern = @"\,\d+|[\)\(;]|\b\d+\b|[\n]";

    public Result<IEnumerable<SqlTableConfigurationModel>> BuildTablesConfiguration(string sqlCreateTables)
    {
        // Split script into individual 'create table' statements
        var singleLineSqlScript = sqlCreateTables.Replace(Constants.LineSeparator, string.Empty);
        var statements = Regex.Split(singleLineSqlScript, Constants.CreateTable, RegexOptions.IgnoreCase)
            .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim());

        var tableConfigResults = statements.Select(BuildTableConfiguration).ToList();

        return tableConfigResults.Any(tc => tc.IsFailed)
            ? Result.Fail(tableConfigResults.Where(x => x.IsFailed).SelectMany(x => x.Errors))
            : Result.Ok(tableConfigResults.Select(tc => tc.Value));
    }

    private Result<SqlTableConfigurationModel> BuildTableConfiguration(string tableLine)
    {
        var tableName = tableLine.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries).First().Trim();
        var definitions = ToTableDefinitions(tableLine, tableName.Length);

        var columns = definitions
            .Where(sqlRowParser.IsPlainColumn)
            .Select(ToPropertyPattern)
            .Select(sqlRowParser.BuildColumnConfiguration)
            .ToList();

        AddPrimaryKey(columns, definitions);
        AddForeignKeys(columns, definitions, tableName);

        return new SqlTableConfigurationModel
        {
            TableName = tableName,
            Columns = columns,
        };
    }

    private void AddPrimaryKey(IEnumerable<SqlColumnConfigurationModel> columns, IEnumerable<string> definitions)
    {
        var pkDefinition = definitions.FirstOrDefault(definition =>
            definition.Contains(Constants.PrimaryKey, StringComparison.OrdinalIgnoreCase));

        if (pkDefinition == null) return;

        var primaryKey = sqlRowParser.GetSqlPrimaryKeyConfiguration(pkDefinition);

        columns
            .Where(column => primaryKey.SourceColumns.Any(col => col == column.ColumnName))
            .ToList()
            .ForEach(column => column.IsPrimaryKey = true);
    }

    private void AddForeignKeys(IEnumerable<SqlColumnConfigurationModel> columns, IEnumerable<string> definitions, string sourceTable)
    {
        var foreignKeys = definitions.Select(sqlRowParser.BuildRelationConfiguration);
        var foreignKeysLookup = foreignKeys.Where(fk => fk is not null)
            .SelectMany(fk => fk.SourceColumns, (fk, sourceColumn) => new { sourceColumn, fk })
            .ToLookup(x => x.sourceColumn, x => x.fk);

        foreach (var column in columns)
        {
            var relation = foreignKeysLookup[column.ColumnName].FirstOrDefault();
            if (relation != null)
            {
                column.IsForeignKey = true;
                relation.SourceTable = sourceTable;
                relation.IsOneToOne = column.Unique;
                relation.IsRequired = column.NotNull;
                column.Relation = relation;
            }
        }
    }

    private string ToPropertyPattern(string line)
    {
        return Regex.Replace(line, NotPropertyPattern, string.Empty);
    }

    private IEnumerable<string> ToTableDefinitions(string tableLine, int tableNameLength)
    {
        var otherItems = tableLine[tableNameLength..];
        var withoutBraces = otherItems.Trim().Substring(1, otherItems.Length - 4);
        var definitions = withoutBraces
            .Split(Constants.ComaSeparator, StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x));
        return definitions;
    }
}
