namespace GenApp.Parsers.Abstractions.Models;

public class SqlTableConfigurationModel
{
    public string TableName { get; set; }

    public ICollection<SqlColumnConfigurationModel> Columns { get; set; }
}
