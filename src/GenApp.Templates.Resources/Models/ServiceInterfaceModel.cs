using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class ServiceInterfaceModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.ServiceInterface;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }

    public string InterfaceName { get; set; }

    public string ModelName { get; set; }

    public string CommandModelName { get; set; }

    public string KeyType { get; set; }
}
