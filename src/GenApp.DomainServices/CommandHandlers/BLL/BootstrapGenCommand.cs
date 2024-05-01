using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class BootstrapGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var injections = model.Entities.AddIdFilter().Select(entity => new InjectionDto
        {
            InterfaceName = $"I{entity.EntityName}Service",
            ClassName = $"{entity.EntityName}Service",
        });

        await fileGenService.CreateEntryAsync(
            archive,
            $"Bootstrap.cs".ToBllProjectFile(model.AppName),
            new BllBootstrapModel
            {
                Namespace = $"{model.AppName}.BLL",
                Injections = injections,
                Usings = new[]
                {
                    "Microsoft.Extensions.DependencyInjection",
                    $"{model.AppName}.BLL.Interfaces",
                    $"{model.AppName}.BLL.Services",
                }.Order(),
            },
            token);
    }
}
