using System.Text.RegularExpressions;
using GenApp.Domain.Enums;
using GenApp.Parsers.Abstractions.Constants;

namespace GenApp.Parsers.CSharp.Mappers;
public static class PropertyTypeMapper
{
    public static string Map(DbmsType dbmsType, string sqlType)
    {
        var regex = new Regex(@"\w+");

        var match = regex.Match(sqlType);
        if (!match.Success)
        {
            throw new ArgumentException($"{sqlType} is not a valid type");
        }

        return dbmsType switch
        {
            DbmsType.MSSQLSERVER => MapForDbms(sqlType, DotnetMsSqlServerTypes.Value),
            DbmsType.MYSQL => MapForDbms(sqlType, DotnetMySqlTypes.Value),
            DbmsType.POSTGRESQL => MapForDbms(sqlType, DotnetPostgreSqlTypes.Value),
            _ => throw new ArgumentException("Not supported DBMS type"),
        };
    }

    private static string MapForDbms(string sqlType, Dictionary<string, IEnumerable<string>> convertion)
    {
        var type = convertion.FirstOrDefault(e => e.Value.Any(value => value.Equals(sqlType, StringComparison.OrdinalIgnoreCase)));

        return type.Key;
    }
}
