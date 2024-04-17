namespace GenApp.Templates.Resources.DTOs;
public class DotnetPropertyDto
{
    public string Name { get; set; }

    public string Type { get; set; }

    public string Nullable { get; set; }

    public string IsRequired { get; set; }

    public bool IsNavigation { get; set; }

    public bool IsCollectionNavigation { get; set; }
}
