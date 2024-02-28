using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class DomainEntityModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.DomainEntity;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string EntityName { get; set; }

    public string KeyType { get; set; }

    public IEnumerable<DotnetPropertyModel> Properties { get; set; }
}
