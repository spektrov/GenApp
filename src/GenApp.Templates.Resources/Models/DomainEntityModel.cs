using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class DomainEntityModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.DomainEntity;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string EntityName { get; set; }

    public string KeyType { get; set; }

    public bool HasId { get; set; }

    public ICollection<DotnetPropertyDto> Properties { get; set; }
}
