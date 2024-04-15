namespace GenApp.Domain.Constants;
public static class DotnetFilterTypes
{
    public static IEnumerable<string> Filter =>
    [
        DotnetTypes.Bool,
        DotnetTypes.String,
        DotnetTypes.Byte,
        DotnetTypes.Short,
        DotnetTypes.Int,
        DotnetTypes.Long,
        DotnetTypes.Float,
        DotnetTypes.Double,
        DotnetTypes.Decimal,
        DotnetTypes.Guid,
        DotnetTypes.DateTime,
        DotnetTypes.DateTimeOffset,
        DotnetTypes.TimeSpan,
    ];

    public static IEnumerable<string> Search =>
    [
        DotnetTypes.String,
    ];

    public static IEnumerable<string> Range =>
    [
        DotnetTypes.Int,
        DotnetTypes.Long,
        DotnetTypes.Float,
        DotnetTypes.Double,
        DotnetTypes.Decimal,
        DotnetTypes.DateTime,
        DotnetTypes.DateTimeOffset,
        DotnetTypes.TimeSpan,
    ];

    public static IEnumerable<string> Sort =>
    [
        DotnetTypes.Bool,
        DotnetTypes.String,
        DotnetTypes.Byte,
        DotnetTypes.Short,
        DotnetTypes.Int,
        DotnetTypes.Long,
        DotnetTypes.Float,
        DotnetTypes.Double,
        DotnetTypes.Decimal,
        DotnetTypes.DateTime,
        DotnetTypes.DateTimeOffset,
        DotnetTypes.TimeSpan,
    ];
}