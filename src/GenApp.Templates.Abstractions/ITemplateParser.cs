namespace GenApp.Templates.Abstractions;

public interface ITemplateParser
{
    Task<string> ParseAsync<T>(T model, CancellationToken token)
        where T : BaseTemplateModel;
}