using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class PagedResponseModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.PagedResponse;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}