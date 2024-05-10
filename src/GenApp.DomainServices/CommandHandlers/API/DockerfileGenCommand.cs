using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class DockerfileGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            $"Dockerfile".ToApiProjectFile(model.AppName),
            new DockerfileModel
            {
                SolutionName = $"{model.AppName}",
                DotnetSdkVersion = $"{model.DotnetSdkVersion}.0"
            },
            token);
    }
}
