namespace GenApp.Domain.Models;
public class SqlColumnConfigurationModel
{
    public string ColumnName { get; set; }

    public string ColumnType { get; set; }

    public bool NotNull { get; set; }

    public bool IsPrimaryKey { get; set; }
}
