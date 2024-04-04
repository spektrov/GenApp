using GenApp.Parsers.Abstractions.Models;
using GenApp.Parsers.Sql.Interfaces;

namespace GenApp.Parsers.Sql.Services;
internal class SqlRowParser : ISqlRowParser
{
    public bool IsPlainColumn(string columnDefinition)
    {
        return !Constants.Relations.Any(c => columnDefinition.Contains(c, StringComparison.OrdinalIgnoreCase));
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
            IsPrimaryKey = DefineIfPrimaryKey(columnLine),
        };

        return column;
    }

    public SqlRelationConfiguration BuildRelationConfiguration(string columnLine)
    {
        return default;
    }

    private bool DefineIfPrimaryKey(string columnDefinition)
    {
        return columnDefinition.Contains(Constants.PrimaryKey, StringComparison.OrdinalIgnoreCase);
    }

    private bool DefineIfForeignKey(string columnDefinition)
    {
        return columnDefinition.Contains(Constants.ForeignKey, StringComparison.OrdinalIgnoreCase);
    }

    private bool DefineIfNotNull(string columnDefinition)
    {
        var isNotNull = columnDefinition.Contains(Constants.NotNull, StringComparison.OrdinalIgnoreCase)
            || columnDefinition.Contains(Constants.Unique, StringComparison.OrdinalIgnoreCase)
            || DefineIfPrimaryKey(columnDefinition);

        return isNotNull;
    }
}
