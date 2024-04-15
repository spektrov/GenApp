using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class BllAutoMapperModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.BllAutoMapper;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }

    public ICollection<BllMappingModelDto> Models { get; set; }
}
