using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class ServiceInterfacesGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(
        ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities.AddIdFilter())
        {
            var fileName = $"Interfaces/I{entity.EntityName}Service.cs";

            await fileGenService.CreateEntryAsync(
                archive,
                fileName.ToBllProjectFile(model.AppName),
                new ServiceInterfaceModel
                {
                    Namespace = $"{model.AppName}.BLL.Interfaces",
                    InterfaceName = $"I{entity.EntityName}Service",
                    ModelName = $"{entity.EntityName}Model",
                    CommandModelName = $"{entity.EntityName}CommandModel",
                    KeyType = entity.IdType,
                    Usings = new List<string>
                    {
                        $"{model.AppName}.BLL.CommandModels",
                        $"{model.AppName}.BLL.DomainModels",
                        $"{model.AppName}.BLL.Models",
                    }.Order(),
                },
                token);
        }
    }
}
