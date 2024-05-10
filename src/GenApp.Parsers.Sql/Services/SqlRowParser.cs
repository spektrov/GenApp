using System.Text.RegularExpressions;
using GenApp.Parsers.Abstractions.Constants;
using GenApp.Parsers.Abstractions.Models;
using GenApp.Parsers.Sql.Extensions;
using GenApp.Parsers.Sql.Interfaces;

namespace GenApp.Parsers.Sql.Services;
internal class SqlRowParser : ISqlRowParser
{
    public bool IsPlainColumn(string columnDefinition)
    {
        return !Constants.Constraints.Any(c => columnDefinition.StartsWith(c, StringComparison.OrdinalIgnoreCase));
    }

    public SqlColumnConfigurationModel BuildColumnConfiguration(string columnLine)
    {
        // Split column line into components and remove leading/trailing white space from each component
        var columnComponents = columnLine.Split(Constants.SpaceSeparator)
            .Select(component => component.Trim()).ToList();

        var column = new SqlColumnConfigurationModel
        {
            ColumnName = columnComponents[0].GetNameWithoutQuotes(), // First component should be column name
            ColumnType = columnComponents[1], // Second component should be column type
            NotNull = DefineIfNotNull(columnLine),
            Unique = DefineIfUnique(columnLine),
        };

        return column;
    }

    public SqlPrimaryKeyConfiguration GetSqlPrimaryKeyConfiguration(string pkDefinition)
    {
        var columnNames = pkDefinition != null
            ? DefinePrimaryKeyColumn(pkDefinition)
            : Enumerable.Empty<string>();

        return new SqlPrimaryKeyConfiguration
        {
            SourceColumns = columnNames,
        };
    }

    public SqlRelationConfiguration? BuildRelationConfiguration(string columnLine)
    {
        if (!IsForeignKey(columnLine))
        {
            return default;
        }

        var re1 = new Regex(
            @"(?i)FOREIGN\s+KEY\s*\((?<SourceColumns>[^\)]+)\)\s*REFERENCES\s*(?<TargetTable>\w+)\s*\((?<TargetColumns>[^\)]+)\)",
            RegexOptions.Compiled);
        var re2 = new Regex(
            @"(?i)(?<SourceColumns>\w+)\s+\w+\s*REFERENCES\s*(?<TargetTable>\w+)\s*\((?<TargetColumns>[^\)]+)\)",
            RegexOptions.Compiled);
        var re3 = new Regex(
            @"(?i)(?<SourceColumns>\w+)\s+\w+\s*FOREIGN\s+KEY\s*REFERENCES\s*(?<TargetTable>\w+)\s*\((?<TargetColumns>[^\)]+)\)",
            RegexOptions.Compiled);

        Match match = re1.Match(columnLine);

        if (!match.Success)
        {
            match = re2.Match(columnLine);
        }

        if (!match.Success)
        {
            match = re3.Match(columnLine);
        }

        if (!match.Success)
        {
            return default;
        }

        var config = new SqlRelationConfiguration
        {
            TargetTable = match.Groups["TargetTable"].Value.GetNameWithoutQuotes(),
            SourceColumns = match.Groups["SourceColumns"].Value.SplitBySeparator(Constants.ComaSeparator).Select(x => x.GetNameWithoutQuotes()),
            TargetColumns = match.Groups["TargetColumns"].Value.SplitBySeparator(Constants.ComaSeparator).Select(x => x.GetNameWithoutQuotes()),
            OnDeleteAction = DefineOnDeleteAction(columnLine),
        };

        return config;
    }

    private string? DefineOnDeleteAction(string columnLine)
    {
        string? onDeleteAction = null;
        if (columnLine.Contains(Constants.OnDelete, StringComparison.OrdinalIgnoreCase))
        {
            onDeleteAction = SqlOnDeleteActions.All.FirstOrDefault(action =>
                columnLine.Contains(action, StringComparison.OrdinalIgnoreCase));
        }

        return onDeleteAction;
    }

    private bool IsForeignKey(string columnDefinition)
    {
        return columnDefinition.Contains(Constants.References, StringComparison.OrdinalIgnoreCase);
    }

    private bool DefineIfNotNull(string columnDefinition)
    {
        var isNotNull = columnDefinition.Contains(Constants.NotNull, StringComparison.OrdinalIgnoreCase)
            || columnDefinition.Contains(Constants.Unique, StringComparison.OrdinalIgnoreCase)
            || columnDefinition.Contains(Constants.PrimaryKey, StringComparison.OrdinalIgnoreCase);

        return isNotNull;
    }

    private bool DefineIfUnique(string columnDefinition)
    {
        var isUnique = columnDefinition.Contains(Constants.Unique, StringComparison.OrdinalIgnoreCase)
            || columnDefinition.Contains(Constants.PrimaryKey, StringComparison.OrdinalIgnoreCase);

        return isUnique;
    }

    private IEnumerable<string> DefinePrimaryKeyColumn(string defenition)
    {
        if (defenition.StartsWith(Constants.PrimaryKey, StringComparison.OrdinalIgnoreCase)
                    || defenition.StartsWith(Constants.Constraint, StringComparison.OrdinalIgnoreCase))
        {
            var pkColumns = GetValueInParenthesis(defenition) ?? string.Empty;
            return pkColumns.SplitBySeparator(Constants.ComaSeparator).Select(x => x.GetNameWithoutQuotes());
        }

        return defenition.SplitBySeparator(Constants.SpaceSeparator).Select(x => x.GetNameWithoutQuotes()).Take(1);
    }

    private string? GetValueInParenthesis(string str)
    {
        string pattern = @"\(([^)]*)\)";
        Match match = Regex.Match(str, pattern);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        else
        {
            return null;
        }
    }
}
