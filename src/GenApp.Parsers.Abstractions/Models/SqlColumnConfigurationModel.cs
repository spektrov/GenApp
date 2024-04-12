namespace GenApp.Parsers.Abstractions.Models;
public class SqlColumnConfigurationModel
{
    required public string ColumnName { get; set; }

    required public string ColumnType { get; set; }

    public bool NotNull { get; set; }

    public bool Unique { get; set; }

    public bool IsPrimaryKey { get; set; }

    public bool IsForeignKey { get; set; }

    public SqlRelationConfiguration? Relation { get; set; }
}