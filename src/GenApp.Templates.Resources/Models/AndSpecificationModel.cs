using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class AndSpecificationModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.AndSpecification;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}