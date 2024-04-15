using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class PaginationParametersModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.PaginationParameters;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}