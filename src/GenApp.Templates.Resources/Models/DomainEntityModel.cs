using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class DomainEntityModel : BaseTemplateModel
{
    public override string TemplateName => TemplateNames.DomainEntity;

    public string Namespace { get; set; }

    public string EntityName { get; set; }

    public string KeyType { get; set; }

    public IEnumerable<DotnetPropertyModel> Properties { get; set; }
}
