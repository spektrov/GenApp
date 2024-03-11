using GenApp.Templates.Abstractions;
using GenApp.Templates.Resources.DTOs;

namespace GenApp.Templates.Resources.Models;
public class ProjectFileModel : BaseTemplateModel
{
    public override string TemplateName => TemplateNames.CsprojFile;

    required public string Type { get; set; }

    required public string SdkVersion { get; set; }

    public IEnumerable<PackageDto>? Packages { get; set; }

    public IEnumerable<string>? Includes { get; set; }
}
