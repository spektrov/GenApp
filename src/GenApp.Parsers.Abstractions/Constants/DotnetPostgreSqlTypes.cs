using GenApp.Domain.Constants;

namespace GenApp.Parsers.Abstractions.Constants;
public static class DotnetPostgreSqlTypes
{
    public static Dictionary<string, IEnumerable<string>> Value => new()
    {
        { DotnetTypes.Bool, new[] { PostgreSqlTypes.Bool } },
        { DotnetTypes.String, new[] { PostgreSqlTypes.Char, PostgreSqlTypes.Varchar, PostgreSqlTypes.Text } },
        { DotnetTypes.Short, new[] { PostgreSqlTypes.Smallint } },
        { DotnetTypes.Int, new[] { PostgreSqlTypes.Int, PostgreSqlTypes.Serial, PostgreSqlTypes.Integer } },
        { DotnetTypes.Long, new[] { PostgreSqlTypes.Bigint } },
        { DotnetTypes.Float, new[] { PostgreSqlTypes.Real } },
        { DotnetTypes.Decimal, new[] { PostgreSqlTypes.Decimal, PostgreSqlTypes.Numeric } },
        { DotnetTypes.ByteArray, new[] { PostgreSqlTypes.Bytea } },
        { DotnetTypes.Guid, new[] { PostgreSqlTypes.Uuid } },
        { DotnetTypes.DateTime, new[] { PostgreSqlTypes.Timestamp, PostgreSqlTypes.TimestampTZ, PostgreSqlTypes.Date } },
        { DotnetTypes.TimeSpan, new[] { PostgreSqlTypes.Time, PostgreSqlTypes.TimeTZ, PostgreSqlTypes.Interval } }
    };
}
