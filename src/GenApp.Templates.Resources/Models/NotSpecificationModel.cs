using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class NotSpecificationModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.NotSpecification;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}