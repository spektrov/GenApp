using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class RepositoryInterfaceModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.EntityRepositoryInterface;

    public string Name { get; set; }

    public string EntityName { get; set; }

    public string KeyType { get; set; }

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
