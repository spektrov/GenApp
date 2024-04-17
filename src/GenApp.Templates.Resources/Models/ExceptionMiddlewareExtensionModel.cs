using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class ExceptionMiddlewareExtensionModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.ExceptionMiddlewareExtension;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
