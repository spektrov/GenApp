using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.API;
internal class AutoMapperGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var models = model.Entities.AddIdFilter().Select(x => new ApiMappingModelDto
        {
            CommandModelName = $"{x.EntityName}CommandModel",
            ModelName = $"{x.EntityName}Model",
            CreateRequestName = $"{x.EntityName}CreateRequest",
            UpdateRequestName = $"{x.EntityName}UpdateRequest",
            ResponseName = $"{x.EntityName}Response",
        }).ToList();

        var usings = new List<string>
        {
            "AutoMapper",
            $"{model.AppName}.API.Models.Responses",
            $"{model.AppName}.BLL.CommandModels",
            $"{model.AppName}.BLL.DomainModels",
        };

        usings.AddRange(model.Entities.AddIdFilter().Select(x =>
            $"{model.AppName}.API.Models.Requests.{x.EntityName}Requests"));

        await fileGenService.CreateEntryAsync(
            archive,
            "AutoMapper/Mapper.cs".ToApiProjectFile(model.AppName),
            new ApiAutoMapperModel
            {
                Namespace = $"{model.AppName}.API.AutoMapper",
                Models = models,
                Usings = usings.Order(),
            },
            token);
    }
}
