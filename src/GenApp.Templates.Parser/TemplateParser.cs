using GenApp.Templates.Abstractions;
using RazorEngineCore;

namespace GenApp.Templates.Parser;

public class TemplateParser : ITemplateParser
{
    public async Task<string> ParseAsync<T>(T model, CancellationToken token)
        where T : BaseTemplateModel
    {
        var template = await GetTemplateAsync(model.TemplateName, token);
        var parsedContent = await template.RunAsync(model);

        return parsedContent;
    }

    public async Task<IRazorEngineCompiledTemplate> GetTemplateAsync(string templateName, CancellationToken token)
    {
        var templatePath = GetTemplatePath(templateName);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template not found at '{templatePath}'.");
        }

        var templateContent = await File.ReadAllTextAsync(templatePath, token);

        var razorEngine = new RazorEngine();
        var template = await razorEngine.CompileAsync(templateContent, cancellationToken: token);

        return template;
    }

    private static string GetTemplatePath(string templateName)
    {
        var templatesDirectory = @"..\GenApp.Templates.Resources\Templates";
        var templateFile = $"{templateName}.cshtml";

        return Path.Combine(templatesDirectory, templateFile);
    }
}