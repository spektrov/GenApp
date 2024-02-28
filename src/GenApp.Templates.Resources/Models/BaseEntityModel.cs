using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class BaseEntityModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.BaseEntity;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
