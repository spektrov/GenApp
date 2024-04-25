using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.StaticTemplates;

namespace GenApp.DomainServices.CommandHandlers.SolutionLevel;

internal class SolutionGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
           archive,
           $"{model.AppName}Solution.sln".ToCoreSolutionFile(),
           SolutionFileContent.Value(model.AppName),
           token);
    }
}
