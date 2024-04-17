using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class ApiBootstrapModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.ApiBootstrap;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}
