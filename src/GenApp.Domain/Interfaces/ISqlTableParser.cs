using FluentResults;
using GenApp.Domain.Models;

namespace GenApp.Domain.Interfaces;

public interface ISqlTableParser
{
    Result<SqlTableConfigurationModel> BuildTableConfiguration(string tableLine);
}
