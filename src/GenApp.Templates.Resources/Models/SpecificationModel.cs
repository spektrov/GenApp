using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class SpecificationModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.Specification;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}
