using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class CommandModelsGenCommand(IFileGenService fileGenService, IMapper mapper) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities)
        {
            var fileName = $"CommandModels/{entity.EntityName}CommandModel.cs";
            var properties = entity.Properties.Where(x => !x.IsNavigation).OrderByDescending(x => x.IsId);
            var propertyDtos = mapper.Map<IEnumerable<DotnetPropertyDto>>(properties).ToList();

            await fileGenService.CreateEntryAsync(
                archive,
                fileName.ToBllProjectFile(model.AppName),
                new CommandModelModel
                {
                    Namespace = $"{model.AppName}.BLL.CommandModels",
                    ModelName = $"{entity.EntityName}CommandModel",
                    Properties = propertyDtos,
                    Usings = Array.Empty<string>(),
                },
                token);
        }
    }
}
