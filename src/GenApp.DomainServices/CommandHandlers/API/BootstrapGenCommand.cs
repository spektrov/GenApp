using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class BootstrapGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            $"Bootstrap.cs".ToApiProjectFile(model.AppName),
            new ApiBootstrapModel
            {
                Namespace = $"{model.AppName}.API",
                Usings = new[]
                {
                    "System.Text.Json.Serialization",
                    "System.Text.Json",
                    $"{model.AppName}.DAL.Models",
                    $"{model.AppName}.API.Filters",
                    $"{model.AppName}.DAL.CustomMigrations",
                    $"{model.AppName}.DAL",
                }.Order(),
            },
            token);
    }
}
