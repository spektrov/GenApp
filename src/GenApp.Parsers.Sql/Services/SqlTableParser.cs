using System.Text;
using System.Text.RegularExpressions;
using FluentResults;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.Abstractions.Models;
using GenApp.Parsers.Sql.Extensions;
using GenApp.Parsers.Sql.Interfaces;

namespace GenApp.Parsers.Sql.Services;
internal class SqlTableParser(ISqlRowParser sqlRowParser) : ISqlTableParser
{
    private static readonly string NotPropertyPattern = @"\,\s*\d+|[\)\(;]|\b\d+\b|[\n]";

    public Result<IEnumerable<SqlTableConfigurationModel>> BuildTablesConfiguration(string sqlCreateTables)
    {
        // Split script into individual 'create table' statements
        var statements = GetCreateTableStatements(sqlCreateTables);

        var tableConfigResults = statements.Select(BuildTableConfiguration).ToList();

        return tableConfigResults.Any(tc => tc.IsFailed)
            ? Result.Fail(tableConfigResults.Where(x => x.IsFailed).SelectMany(x => x.Errors))
            : Result.Ok(tableConfigResults.Select(tc => tc.Value));
    }

    private Result<SqlTableConfigurationModel> BuildTableConfiguration(string tableLine)
    {
        var (tableName, keepCase) = GetTableName(tableLine);
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
            KeepCase = keepCase,
            Columns = columns,
        };
    }

    private void AddPrimaryKey(IEnumerable<SqlColumnConfigurationModel> columns, IEnumerable<string> definitions)
    {
        var pkDefinition = definitions.FirstOrDefault(definition =>
            definition.Contains(Constants.PrimaryKey, StringComparison.OrdinalIgnoreCase));

        if (pkDefinition == null) return;

        var pkConfiguration = sqlRowParser.GetSqlPrimaryKeyConfiguration(pkDefinition);

        if (pkConfiguration == null || !pkConfiguration.HasPK) return;

        columns
            .Where(column => pkConfiguration.SourceColumns.Any(col => col == column.ColumnName))
            .ToList()
            .ForEach(column => column.IsPrimaryKey = true);
    }

    private void AddForeignKeys(IEnumerable<SqlColumnConfigurationModel> columns, IEnumerable<string> definitions, string sourceTable)
    {
        var foreignKeys = definitions.Select(sqlRowParser.BuildRelationConfiguration);
        var foreignKeysLookup = foreignKeys.Where(fk => fk is not null && fk.SourceColumns.Count() == 1)
            .SelectMany(fk => fk.SourceColumns, (fk, sourceColumn) => new { sourceColumn, fk })
            .ToLookup(x => x.sourceColumn, x => x.fk);

        foreach (var column in columns)
        {
            var relation = foreignKeysLookup[column.ColumnName].FirstOrDefault();
            var sameFKTable = relation is not null &&
                foreignKeys.Where(fk => fk is not null && fk.SourceColumns.Count() == 1)
                .Any(x => x.TargetTable == relation.TargetTable && !x.SourceColumns.SequenceEqual(relation.SourceColumns));
            if (relation != null)
            {
                column.IsForeignKey = true;
                relation.SourceTable = sourceTable;
                relation.IsOneToOne = column.Unique;
                relation.IsRequired = column.NotNull;
                relation.HasManyFKToOneTable = sameFKTable;
                column.Relation = relation;
            }
        }
    }

    private (string, bool) GetTableName(string tableLine)
    {
        var rawName = tableLine
            .Split(Constants.OpenBracesSeparator, StringSplitOptions.RemoveEmptyEntries)
            .First()
            .Trim();

        var name = rawName.GetNameWithoutQuotes(true);
        var keepCase = rawName.ValueInQuotes();

        return (name, keepCase);
    }

    private IEnumerable<string> GetCreateTableStatements(string sqlCreateTables)
    {
        var preparedScript = PrepareSingleLineScript(sqlCreateTables);
        var statements = Regex.Split(preparedScript, Constants.CreateTable, RegexOptions.IgnoreCase)
           .Where(s => !string.IsNullOrWhiteSpace(s))
           .Select(s => Regex.Replace(s, Constants.IfNotExists, string.Empty, RegexOptions.IgnoreCase))
           .Select(s => s.Trim());

        return statements;
    }

    private string PrepareSingleLineScript(string sqlCreateTables)
    {
        var singleLineSqlScript = Regex.Replace(sqlCreateTables, @"\r\n?|\n", string.Empty);
        var singleSpacesSqlScript = Regex.Replace(singleLineSqlScript, @"\s+", " ");

        return singleSpacesSqlScript;
    }

    private string ToPropertyPattern(string line)
    {
        return Regex.Replace(line, NotPropertyPattern, string.Empty);
    }

    private IEnumerable<string> ToTableDefinitions(string tableLine, int tableNameLength)
    {
        var otherItems = tableLine[tableNameLength..];

        var openBraceIndex = otherItems.IndexOf(Constants.OpenBracesSeparator);
        otherItems = openBraceIndex >= 0 ? otherItems[(openBraceIndex + 1)..] : otherItems;

        var closeBraceIndex = otherItems.LastIndexOf(Constants.CloseBracesSeparator);
        otherItems = closeBraceIndex > 0 ? otherItems[..closeBraceIndex] : otherItems;

        var replacedCommas = ReplaceCommasInBraces(otherItems, Constants.SpecialSymbol);

        var definitions = replacedCommas.Split(Constants.ComaSeparator, StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(def => def.Replace(Constants.SpecialSymbol, Constants.ComaSeparator))
            .ToList();

        return definitions;
    }

    private string ReplaceCommasInBraces(string input, char specialSymbol)
    {
        var buffer = new StringBuilder();
        bool inParenthesis = false;

        foreach (var ch in input)
        {
            if (ch == Constants.OpenBracesSeparator)
            {
                inParenthesis = true;
            }
            else if (ch == Constants.CloseBracesSeparator)
            {
                inParenthesis = false;
            }

            if (ch == Constants.ComaSeparator && inParenthesis)
            {
                buffer.Append(specialSymbol);
            }
            else
            {
                buffer.Append(ch);
            }
        }

        return buffer.ToString();
    }
}
