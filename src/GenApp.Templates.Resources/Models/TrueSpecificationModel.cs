using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class TrueSpecificationModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.TrueSpecification;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}