using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class AppJsonSerializerOptionsModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.AppJsonSerializerOptions;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}
