using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class DalBootstrapModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.DalBootstrap;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string DbmsUsage { get; set; }

    public IEnumerable<InjectionDto> Injections { get; set; }
}
