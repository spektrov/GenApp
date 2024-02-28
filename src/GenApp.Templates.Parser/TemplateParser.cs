using GenApp.Templates.Abstractions;
using RazorEngineCore;

namespace GenApp.Templates.Parser;

public class TemplateParser : ITemplateParser
{
    public async Task<string> ParseAsync<T>(T model, CancellationToken token)
        where T : BaseTemplateModel
    {
        var templatePath = GetTemplatePath(model.TemplateName);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template not found at '{templatePath}'.");
        }

        var templateContent = await File.ReadAllTextAsync(templatePath, token);

        var razorEngine = new RazorEngine();
        var template = await razorEngine.CompileAsync(templateContent, cancellationToken: token);
        var parsedContent = await template.RunAsync(model);

        return parsedContent;
    }

    private static string GetTemplatePath(string templateName)
    {
        var templatesDirectory = @"..\GenApp.Templates.Resourses\Templates";
        var templateFile = $"{templateName}.cshtml";

        return Path.Combine(templatesDirectory, templateFile);
    }
}