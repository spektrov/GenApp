using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class SpecificationExtensionsModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.SpecificationExtensions;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}