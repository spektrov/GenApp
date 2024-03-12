using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class DomainEntitiesGenCommand(IFileGenService fileGenService, IMapper mapper) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var usingList = new List<string> { $"{model.AppName}.DAL.Interfaces" };
        foreach (var entity in model.Entities)
        {
            var fileName = $"Entities/{entity.EntityName}Entity.cs";
            await fileGenService.CreateEntryAsync(
                archive,
                fileName.ToDalProjectFile(model.AppName),
                new DomainEntityModel
                {
                    Namespace = $"{model.AppName}.DAL.Entities",
                    EntityName = $"{entity.EntityName}Entity",
                    KeyType = entity.Properties.FirstOrDefault(x => x.IsId)?.Type,
                    Properties = mapper.Map<IEnumerable<DotnetPropertyDto>>(entity.Properties),
                    Usings = usingList,
                },
                token);
        }
    }
}
