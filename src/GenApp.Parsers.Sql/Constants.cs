namespace GenApp.Parsers.Sql;
public static class Constants
{
    public const string CreateTable = "create table";
    public const string ForeignKey = "foreign key";
    public const string References = "references";
    public const string Constraint = "constraint";
    public const string NotNull = "not null";
    public const string Unique = "unique";
    public const string Check = "check";
    public const string PrimaryKey = "primary key";
    public const string OnDelete = "on delete";
    public const char ComaSeparator = ',';
    public const char SpaceSeparator = ' ';
    public const char OpenBracesSeparator = '(';
    public const char CloseBracesSeparator = ')';
    public const char SpecialSymbol = '>';
    public const string LineSeparator = "\n";

    public static readonly string[] Constraints =
        [Constraint, ForeignKey, Unique, PrimaryKey, Check];

    public static readonly string[] Relations =
        [ForeignKey, PrimaryKey, References, Constraint];
}
