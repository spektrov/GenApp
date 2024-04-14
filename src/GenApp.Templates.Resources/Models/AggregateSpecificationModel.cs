using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class AggregateSpecificationModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.AggregateSpecification;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}