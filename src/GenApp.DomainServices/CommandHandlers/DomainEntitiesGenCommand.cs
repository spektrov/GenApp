﻿using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers;
internal class DomainEntitiesGenCommand(IFileGenService fileGenService, IMapper mapper) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var usingList = new List<string> { $"{model.AppName}.Domain.Interfaces" };
        foreach (var entity in model.Entities)
        {
            var fileName = $"Entities/{entity.EntityName}Entity.cs";
            await fileGenService.CreateEntryAsync(
                archive,
                fileName.ToDomainProjectFile(model.AppName),
                new DomainEntityModel
                {
                    Namespace = $"{model.AppName}.Domain.Entities",
                    EntityName = $"{entity.EntityName}Entity",
                    KeyType = entity.Properties.FirstOrDefault(x => x.IsId)?.Type,
                    Properties = mapper.Map<IEnumerable<DotnetPropertyModel>>(entity.Properties),
                    Usings = usingList,
                },
                token);
        }
    }
}
