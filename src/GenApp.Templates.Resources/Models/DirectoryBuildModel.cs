using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class DirectoryBuildModel : BaseTemplateModel
{
    public override string TemplateName => TemplateNames.DirectoryBuild;

    public string SdkVersion { get; set; }
}
