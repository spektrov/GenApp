using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class PageCountCalcucatorModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.PageCountCalculator;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}
