namespace GenApp.Domain.Models;
public class DotnetEntityConfigurationModel
{
    public string EntityName { get; set; }

    public IEnumerable<DotnetPropertyConfigurationModel> Properties { get; set; }
}
