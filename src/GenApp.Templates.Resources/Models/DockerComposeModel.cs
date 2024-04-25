using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class DockerComposeModel : BaseTemplateModel
{
    public override string TemplateName => TemplateNames.DockerCompose;

    public string DbServiceName { get; set; }

    public string DbImageName { get; set; }

    public string DbPorts { get; set; }

    public string VolumesValue { get; set; }

    public IEnumerable<VariableDto> EnvVariables { get; set; }

    public string DockerProjectName { get; set; }
}