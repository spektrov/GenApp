using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class SortingOrderCalculatorModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.SortingOrderCalculator;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}
