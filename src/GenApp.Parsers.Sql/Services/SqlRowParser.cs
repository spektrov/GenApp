using System.Text.RegularExpressions;
using GenApp.Parsers.Abstractions.Models;
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
            ColumnName = columnComponents[0], // First component should be column name
            ColumnType = columnComponents[1], // Second component should be column type
            NotNull = DefineIfNotNull(columnLine),
            Unique = DefineIfUnique(columnLine),
        };

        return column;
    }

    public SqlPrimaryKeyConfiguration GetSqlPrimaryKeyConfiguration(string pkDefenition)
    {
        var columnNames = pkDefenition != null
            ? DefinePrimaryKeyColumn(pkDefenition)
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

        var re1 = new Regex(@"FOREIGN KEY \((?<SourceColumns>[^\)]+)\) REFERENCES (?<TargetTable>\w+)\((?<TargetColumns>[^\)]+)\)");
        var re2 = new Regex(@"(?<SourceColumns>\w+) \w+ REFERENCES (?<TargetTable>\w+)\((?<TargetColumns>[^\)]+)\)");
        var re3 = new Regex(@"(?<SourceColumns>\w+) \w+ FOREIGN KEY REFERENCES (?<TargetTable>\w+)\((?<TargetColumns>[^\)]+)\)");

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
            TargetTable = match.Groups["TargetTable"].Value,
            SourceColumns = SplitBySeparator(match.Groups["SourceColumns"].Value, Constants.ComaSeparator),
            TargetColumns = SplitBySeparator(match.Groups["TargetColumns"].Value, Constants.ComaSeparator),
        };

        return config;
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
            var pkColumns = GetValueInParenthesis(defenition);
            return SplitBySeparator(pkColumns, Constants.ComaSeparator);
        }

        return SplitBySeparator(defenition, Constants.SpaceSeparator).Take(1);
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

    private IEnumerable<string> SplitBySeparator(string str, char separator)
    {
        return !string.IsNullOrWhiteSpace(str)
            ? str.Split(separator).Select(c => c.Trim())
            : Enumerable.Empty<string>();
    }
}
