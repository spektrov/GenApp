using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class EnumSortingOrderModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.EnumSortingOrder;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
