using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class ServiceModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.Service;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string ServiceName { get; set; }

    public string InterfaceName { get; set; }

    public string RepositoryInterfaceName { get; set; }

    public string ModelName { get; set; }

    public string EntityName { get; set; }

    public string CommandModelName { get; set; }

    public string KeyType { get; set; }

    public string FindByIdSpecification { get; set; }

    public ICollection<string>? GetManyIncludes { get; set; }

    public ICollection<string>? GetByIdIncludes { get; set; }

    public ICollection<string> PropertiesForUpdate { get; set; }
}