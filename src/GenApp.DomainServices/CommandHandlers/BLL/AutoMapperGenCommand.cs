using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class AutoMapperGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var models = model.Entities.Select(x => new BllMappingModelDto
        {
            CommandModelName = $"{x.EntityName}CommandModel",
            EntityName = $"{x.EntityName}Entity",
            ModelName = $"{x.EntityName}Model",
        }).ToList();

        await fileGenService.CreateEntryAsync(
            archive,
            "AutoMapper/Mapper.cs".ToBllProjectFile(model.AppName),
            new BllAutoMapperModel
            {
                Namespace = $"{model.AppName}.BLL.AutoMapper",
                Models = models,
                Usings = new List<string>
                {
                    "AutoMapper",
                    $"{model.AppName}.BLL.CommandModels",
                    $"{model.AppName}.BLL.DomainModels",
                    $"{model.AppName}.DAL.Entities",
                }.Order(),
            },
            token);
    }
}
