using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class QueryableExtensionsModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.QueryableExtensions;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
