using System.IO.Compression;
using System.Text;
using GenApp.Domain.Interfaces;
using GenApp.Templates.Abstractions;

namespace GenApp.DomainServices.Services;

public class FileGenService(ITemplateParser templateParser) : IFileGenService
{
    public async Task CreateEntryAsync(
        ZipArchive archive,
        string entryName,
        BaseTemplateModel model,
        CancellationToken token)
    {
        var entry = archive.CreateEntry(entryName);
        await using var entryStream = entry.Open();
        var content = await templateParser.ParseAsync(model, token);
        var bytes = Encoding.UTF8.GetBytes(content);
        await entryStream.WriteAsync(bytes, token);
    }

    public async Task CreateEntryAsync(
        ZipArchive archive,
        string entryName,
        string content,
        CancellationToken token)
    {
        var entry = archive.CreateEntry(entryName);
        await using var entryStream = entry.Open();
        var bytes = Encoding.UTF8.GetBytes(content);
        await entryStream.WriteAsync(bytes, token);
    }
}
