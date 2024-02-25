using GenApp.Domain.Enums;

namespace GenApp.Domain.Models;

public class SqlTableConfigurationModel
{
    public string TableName { get; set; }

    public IEnumerable<SqlColumnConfigurationModel> Columns { get; set; }

    public DbmsType DbmsType { get; set; }
}
