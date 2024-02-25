using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.StaticTemplates;

namespace GenApp.DomainServices.CommandHandlers;

internal class SolutionGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ExtendedGenSettingsModel model, CancellationToken token)
    {
        var fileName = $"{model.AppName}Solution.sln";

        return fileGenService.CreateEntryAsync(
           archive,
           fileName.ToCoreSolutionFile(),
           SolutionFileContent.Value(model.AppName),
           token);
    }
}
