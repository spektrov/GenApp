using FluentResults;
using GenApp.Domain.Enums;
using GenApp.Domain.Models;
using GenApp.Parsers.Abstractions.Models;

namespace GenApp.Parsers.Abstractions.Interfaces;
public interface IDotnetEntityFactory
{
    Result<IEnumerable<DotnetEntityConfigurationModel>> Create(IEnumerable<SqlTableConfigurationModel> tables, DbmsType dbms);
}
