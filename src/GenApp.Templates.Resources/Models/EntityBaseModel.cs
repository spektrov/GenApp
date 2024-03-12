using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class EntityBaseModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.IBaseEntity;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
