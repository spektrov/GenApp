namespace GenApp.Domain.Models;
public class DotnetEntityConfigurationModel
{
    required public string EntityName { get; set; }

    public IEnumerable<DotnetPropertyConfigurationModel> Properties { get; set; }
}
