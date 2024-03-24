using FluentResults;
using GenApp.Domain.Models;
using GenApp.Parsers.Abstractions.Models;

namespace GenApp.Parsers.Abstractions.Interfaces;

public interface ISqlTableParser
{
    Result<IEnumerable<SqlTableConfigurationModel>> BuildTablesConfiguration(string sqlCreateTables);
}
