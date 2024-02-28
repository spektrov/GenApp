using FluentResults;
using GenApp.Domain.Models;

namespace GenApp.Domain.Interfaces;

public interface ISqlTableParser
{
    Result<IEnumerable<SqlTableConfigurationModel>> BuildTablesConfiguration(string sqlCreateTables);
}
