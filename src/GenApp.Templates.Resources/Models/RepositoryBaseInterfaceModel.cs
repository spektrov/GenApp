using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class RepositoryBaseInterfaceModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.BaseRepositoryInterface;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
