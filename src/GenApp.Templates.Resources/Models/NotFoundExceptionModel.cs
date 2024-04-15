using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class NotFoundExceptionModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.NotFoundException;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}