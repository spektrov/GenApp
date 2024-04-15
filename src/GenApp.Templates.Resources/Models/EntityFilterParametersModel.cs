using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class EntityFilterParametersModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.EntityFilterParameters;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }

    public string Name { get; set; }

    public ICollection<FilterPropertyDto> Properties { get; set; }

    public int TotalCount { get => Properties.Count; }
}
