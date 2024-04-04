using System.Text.RegularExpressions;
using FluentResults;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.Abstractions.Models;
using GenApp.Parsers.Sql.Interfaces;

namespace GenApp.Parsers.Sql.Services;
internal class SqlTableParser(ISqlRowParser sqlRowParser) : ISqlTableParser
{
    private static readonly string TablePropertyPattern = @"\,\d+|[\)\(;]|\b\d+\b|[\n]";

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
        var tableName = tableLine.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();
        var allItems = Regex.Replace(tableLine, TablePropertyPattern, string.Empty)[tableName.Length..];

        var items = allItems.Split(Constants.ComaSeparator, StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x));

        var columns = items.Where(sqlRowParser.IsPlainColumn).Select(sqlRowParser.BuildColumnConfiguration);

        var relations = items.Where(x => !sqlRowParser.IsPlainColumn(x)).Select(sqlRowParser.BuildRelationConfiguration);


        return new SqlTableConfigurationModel
        {
            TableName = tableName,
            Columns = columns,
        };
    }
}
