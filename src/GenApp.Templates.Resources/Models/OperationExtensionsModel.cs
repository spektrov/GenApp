using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class OperationExtensionsModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.OperationExtensions;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}