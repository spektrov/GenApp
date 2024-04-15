using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class CommandModelModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.CommandModel;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string ModelName { get; set; }

    public ICollection<DotnetPropertyDto> Properties { get; set; }
}
