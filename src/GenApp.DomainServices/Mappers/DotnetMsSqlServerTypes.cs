using GenApp.Domain.Constants;

namespace GenApp.DomainServices.Mappers;
public static class DotnetMsSqlServerTypes
{
    public static Dictionary<string, IEnumerable<string>> Value => new()
    {
        { DotnetTypes.Bool, new[] { MSSqlServerTypes.Bit } },
        { DotnetTypes.Byte, new[] { MSSqlServerTypes.Byte } },
        { DotnetTypes.Short, new[] { MSSqlServerTypes.SmallInt } },
        { DotnetTypes.Int, new[] { MSSqlServerTypes.Int } },
        { DotnetTypes.Long, new[] { MSSqlServerTypes.BigInt } },
        { DotnetTypes.Decimal, new[] { MSSqlServerTypes.Decimal, MSSqlServerTypes.Numeric, MSSqlServerTypes.Money, MSSqlServerTypes.SmallMoney, } },
        { DotnetTypes.Float, new[] { MSSqlServerTypes.Real } },
        { DotnetTypes.Double, new[] { MSSqlServerTypes.Float } },
        { DotnetTypes.String, new[] { MSSqlServerTypes.Char, MSSqlServerTypes.Varchar, MSSqlServerTypes.Text, MSSqlServerTypes.NChar, MSSqlServerTypes.NVarchar, MSSqlServerTypes.NText, } },
        { DotnetTypes.Guid, new[] { MSSqlServerTypes.UniqueIdentifier } },
        { DotnetTypes.DateTime, new[] { MSSqlServerTypes.Date, MSSqlServerTypes.DateTime, MSSqlServerTypes.DateTime2, MSSqlServerTypes.SmallDateTime } },
        { DotnetTypes.DateTimeOffset, new[] { MSSqlServerTypes.DateTimeOffset } },
        { DotnetTypes.TimeSpan, new[] { MSSqlServerTypes.Time } },
        { DotnetTypes.ByteArray, new[] { MSSqlServerTypes.RowVersion, MSSqlServerTypes.Timestamp } },
    };
}
