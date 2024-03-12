using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class RepositoryBaseModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.BaseRepositoryClass;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
