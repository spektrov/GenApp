using System.Text.RegularExpressions;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;

namespace GenApp.DomainServices.Mappers;

public class SqlTableScriptResolver(ISqlTableParser sqlTableParser)
    : IValueResolver<GenSettingsModel, ExtendedGenSettingsModel, SqlTableConfigurationModel>
{
    private static readonly string createTableSeparator = "create table";

    public SqlTableConfigurationModel Resolve(
        GenSettingsModel source, ExtendedGenSettingsModel destination, SqlTableConfigurationModel destMember, ResolutionContext context)
    {
        // Split script into individual statements
        source.SqlTableScript = source.SqlTableScript.Replace("\n", string.Empty);

        var statements = Regex.Split(source.SqlTableScript, createTableSeparator, RegexOptions.IgnoreCase)
            .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim());

        // TODO: build multiple sql tables.
        var result = sqlTableParser.BuildTableConfiguration(statements.FirstOrDefault());
        if (result.IsFailed)
        {
            return null;
        }

        result.Value.DbmsType = source.DbmsType;

        return result.Value;
    }
}
