using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.StaticTemplates;

namespace GenApp.DomainServices.CommandHandlers;

public class EditorconfigGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var fileName = ".editorconfig";

        return fileGenService.CreateEntryAsync(
            archive,
            fileName.ToCoreSolutionFile(),
            EditorconfigContent.Value,
            token);
    }
}
