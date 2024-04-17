using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class ControllerModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.Controller;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string ControllerName { get; set; }

    public string ServiceInterfaceName { get; set; }

    public string ResponseModelName { get; set; }

    public string CreateRequestName { get; set; }

    public string UpdateRequestName { get; set; }

    public string CommandModelName { get; set; }

    public string KeyType { get; set; }
}