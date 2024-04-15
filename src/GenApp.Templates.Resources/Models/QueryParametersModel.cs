using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class QueryParametersModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.QueryParameters;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}