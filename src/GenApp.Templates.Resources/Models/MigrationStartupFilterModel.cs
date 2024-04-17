using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class MigrationStartupFilterModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.MigrationStartupFilter;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}
