using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class DockerfileModel : BaseTemplateModel
{
    public override string TemplateName => TemplateNames.Dockerfile;

    public string SolutionName { get; set; }

    public string DotnetSdkVersion { get; set; }
}
