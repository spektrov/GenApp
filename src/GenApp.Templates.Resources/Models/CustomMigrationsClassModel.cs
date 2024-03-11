using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class CustomMigrationsClassModel : BaseTemplateModel, ICsharpClassModel
{
	public override string TemplateName => TemplateNames.CustomMigrationsClass;

	public string Namespace { get; set; }

	public IEnumerable<string>? Usings { get; set; }
}
