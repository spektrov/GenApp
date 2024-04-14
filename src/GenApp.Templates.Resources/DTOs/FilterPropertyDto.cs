namespace GenApp.Templates.Resources.DTOs;
public class FilterPropertyDto
{
    public string FilterName { get; set; }

    public string JsonName { get; set; }

    public string Type { get; set; }

    public bool IsRange { get; set; }

    public bool IsFilter { get; set; }

    public bool IsSearch { get; set; }
}
