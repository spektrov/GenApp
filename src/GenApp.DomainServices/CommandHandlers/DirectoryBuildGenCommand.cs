using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers;
internal class DirectoryBuildGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var fileName = "Directory.Build.props";

        return fileGenService.CreateEntryAsync(
            archive,
            fileName.ToCoreSolutionFile(),
            new DirectoryBuildModel { SdkVersion = model.DotnetSdkVersion },
            token);
    }
}
