using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class ProgramModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.Program;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }

    public bool EnableDocker { get; set; }
}
