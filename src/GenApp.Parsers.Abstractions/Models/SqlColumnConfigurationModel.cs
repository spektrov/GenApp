namespace GenApp.Parsers.Abstractions.Models;
public class SqlColumnConfigurationModel
{
    required public string ColumnName { get; set; }

    required public string ColumnType { get; set; }

    public bool NotNull { get; set; }

    public bool IsPrimaryKey { get; set; }

    public bool IsForeignKey { get; set; }
}

public class SqlRelationConfiguration
{
    public string SourceTable { get; set; }

    public string TargetTable { get; set; }

    public string SourceColumn { get; set; }

    public string TargetColumn { get; set; }
}
