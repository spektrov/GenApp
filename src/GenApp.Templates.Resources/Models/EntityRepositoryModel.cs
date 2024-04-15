using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class EntityRepositoryModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.EntityRepository;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string RepositoryName { get; set; }

    public string InterfaceName { get; set; }

    public string EntityName { get; set; }

    public string KeyType { get; set; }

    public string FilterParametersName { get; set; }

    public ICollection<RepositoryFilterDto> FilterParameters { get; set; }

    public string SearchParametersName { get; set; }

    public ICollection<RepositoryFilterDto> SearchParameters { get; set; }

    public string RangeParametersName { get; set; }

    public ICollection<RepositoryFilterDto> RangeParameters { get; set; }

    public ICollection<SortPropertyDto> SortParameters { get; set; }
}