using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class BllBootstrapModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.BllBootstrap;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public IEnumerable<InjectionDto> Injections { get; set; }
}