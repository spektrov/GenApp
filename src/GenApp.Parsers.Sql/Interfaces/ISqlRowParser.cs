using GenApp.Parsers.Abstractions.Models;

namespace GenApp.Parsers.Sql.Interfaces;
public interface ISqlRowParser
{
    bool IsPlainColumn(string columnDefinition);

    SqlColumnConfigurationModel BuildColumnConfiguration(string columnLine);

    SqlRelationConfiguration? BuildRelationConfiguration(string columnLine);

    SqlPrimaryKeyConfiguration GetSqlPrimaryKeyConfiguration(string pkDefinition);
}
