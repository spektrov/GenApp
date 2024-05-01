namespace GenApp.Templates.Resources.DTOs;
public class RelationConfigurationDto
{
    public string NavigationPropertyName { get; set; }

    public string Cardinality { get; set; }

    public string RevertedPropertyName { get; set; }

    public string ForeignPropertyName { get; set; }

    public string IsRequired { get; set; }

    public string OnDeleteAction { get; set; }
}
