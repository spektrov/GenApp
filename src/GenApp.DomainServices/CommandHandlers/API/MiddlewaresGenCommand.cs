using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class MiddlewaresGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            "Middlewares/ExceptionMiddlewareExtension.cs".ToApiProjectFile(model.AppName),
            new ExceptionMiddlewareExtensionModel
            {
                Namespace = $"{model.AppName}.API.Middlewares",
                Usings = new List<string>
                {
                    "Microsoft.AspNetCore.Diagnostics",
                    "System.Net.Mime",
                    $"{model.AppName}.BLL.Exceptions",
                }.Order(),
            },
            token);
    }
}
