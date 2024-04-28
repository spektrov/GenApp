namespace GenApp.Domain.Models;
public class DotnetEntityConfigurationModel
{
    required public string EntityName { get; set; }

    public string? IdType { get; set; }

    public SqlTableInfoModel Table { get; set; }

    public ICollection<DotnetPropertyConfigurationModel> Properties { get; set; }
}
