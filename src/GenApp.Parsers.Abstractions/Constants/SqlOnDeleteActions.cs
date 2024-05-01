namespace GenApp.Parsers.Abstractions.Constants;
public static class SqlOnDeleteActions
{
    public const string Cascade = "CASCADE";
    public const string SetNull = "SET NULL";
    public const string SetDefault = "SET DEFAULT";
    public const string NoAction = "NO ACTION";
    public const string Restrict = "RESTRICT";

    public static string[] All => new[] { Cascade, SetNull, SetDefault, NoAction, Restrict };
}
