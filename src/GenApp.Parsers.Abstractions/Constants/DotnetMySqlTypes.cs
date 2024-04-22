using GenApp.Domain.Constants;

namespace GenApp.Parsers.Abstractions.Constants;
public static class DotnetMySqlTypes
{
    public static Dictionary<string, IEnumerable<string>> Value => new()
    {
        { DotnetTypes.Bool, new[] { MySqlTypes.Bit } },
        { DotnetTypes.Byte, new[] { MySqlTypes.TinyInt, } },
        { DotnetTypes.Short, new[] { MySqlTypes.SmallInt } },
        { DotnetTypes.Int, new[] { MySqlTypes.MediumInt, MySqlTypes.Int, MySqlTypes.AutoIncrement } },
        { DotnetTypes.Long, new[] { MySqlTypes.BigInt } },
        { DotnetTypes.Decimal, new[] { MySqlTypes.Decimal } },
        { DotnetTypes.Float, new[] { MySqlTypes.Float } },
        { DotnetTypes.Double, new[] { MySqlTypes.Double } },
        { DotnetTypes.String, new[] { MySqlTypes.Char, MySqlTypes.Varchar, MySqlTypes.Tinytext, MySqlTypes.Text, MySqlTypes.Mediumtext, MySqlTypes.Longtext, } },
        { DotnetTypes.DateTime, new[] { MySqlTypes.Date, MySqlTypes.DateTime, MySqlTypes.Timestamp } },
        { DotnetTypes.TimeSpan, new[] { MySqlTypes.Time } },
        { DotnetTypes.ByteArray, new[] { MySqlTypes.Binary, MySqlTypes.Varbinary, MySqlTypes.Tinyblob, MySqlTypes.Blob, MySqlTypes.Mediumblob, MySqlTypes.Longblob } },
    };
}
