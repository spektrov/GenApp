namespace GenApp.Domain.Models;
public class DotnetPropertyConfigurationModel
{
    public string Name { get; set; }

    public string Type { get; set; }

    public bool NotNull { get; set; }

    public bool IsId { get; set; }
}
