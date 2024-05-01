using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class EntityConfigurationModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.EntityConfiguration;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string ConfigurationName { get; set; }

    public string EntityName { get; set; }

    public string TableName { get; set; }

    public bool HasPK { get; set; }

    public string IdColumnName { get; set; }

    public ICollection<ColumnConfigurationDto> ColumnConfigs { get; set; }

    public ICollection<RelationConfigurationDto> RelationConfigs { get; set; }
}
