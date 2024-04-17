using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class ProgramGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            $"Program.cs".ToApiProjectFile(model.AppName),
            new ProgramModel
            {
                EnableDocker = model.UseDocker,
                Usings = new[]
                {
                    $"{model.AppName}.API",
                    $"{model.AppName}.API.Middlewares",
                    $"{model.AppName}.BLL",
                    $"{model.AppName}.DAL",
                }.Order(),
            },
            token);
    }
}
