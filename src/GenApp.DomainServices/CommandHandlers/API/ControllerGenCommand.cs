using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class ControllerGenCommand(IFileGenService fileGenService, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities)
        {
            var plural = caseTransformer.ToPascalCase(caseTransformer.ToPlural(entity.EntityName));
            var fileName = $"Controllers/{plural}Controller.cs";

            await fileGenService.CreateEntryAsync(
                archive,
                fileName.ToApiProjectFile(model.AppName),
                new ControllerModel
                {
                    Namespace = $"{model.AppName}.API.Controllers",
                    ControllerName = $"{plural}Controller",
                    ServiceInterfaceName = $"I{entity.EntityName}Service",
                    ResponseModelName = $"{entity.EntityName}Response",
                    CreateRequestName = $"{entity.EntityName}CreateRequest",
                    UpdateRequestName = $"{entity.EntityName}UpdateRequest",
                    CommandModelName = $"{entity.EntityName}CommandModel",
                    KeyType = entity.IdType,
                    Usings = new List<string>
                    {
                        "AutoMapper",
                        "Microsoft.AspNetCore.Mvc",
                        $"{model.AppName}.API.Models.Requests.{entity.EntityName}Requests",
                        $"{model.AppName}.API.Models.Responses",
                        $"{model.AppName}.BLL.CommandModels",
                        $"{model.AppName}.BLL.Interfaces",
                        $"{model.AppName}.BLL.Models",
                    }.Order(),
                },
                token);
        }
    }
}
