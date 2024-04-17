using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class ApiAutoMapperModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.ApiAutoMapper;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }

    public ICollection<ApiMappingModelDto> Models { get; set; }
}
