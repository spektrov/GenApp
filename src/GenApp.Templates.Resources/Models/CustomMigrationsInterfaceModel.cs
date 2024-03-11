using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class CustomMigrationsInterfaceModel : BaseTemplateModel, ICsharpClassModel
{
	public override string TemplateName => TemplateNames.CustomMigrationsInterface;

	public string Namespace { get; set; }

	public IEnumerable<string>? Usings { get; set; }
}
