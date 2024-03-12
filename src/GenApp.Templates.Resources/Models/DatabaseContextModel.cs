using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class DatabaseContextModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.DatabaseContext;

    public IEnumerable<DbSetDto> Sets { get; set; }

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
