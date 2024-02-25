using System.IO.Compression;
using GenApp.Templates.Abstractions;

namespace GenApp.Domain.Interfaces;
public interface IFileGenService
{
    Task CreateEntryAsync(
        ZipArchive archive,
        string entryName,
        BaseTemplateModel model,
        CancellationToken token);

    Task CreateEntryAsync(
        ZipArchive archive,
        string entryName,
        string content,
        CancellationToken token);
}
