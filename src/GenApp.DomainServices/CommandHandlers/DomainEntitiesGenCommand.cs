using System.IO.Compression;
using AutoMapper;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers;
internal class DomainEntitiesGenCommand(IFileGenService fileGenService, IMapper mapper) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ExtendedGenSettingsModel model, CancellationToken token)
    {
        var fileName = $"Entities/{model.EntityConfiguration.EntityName}.cs";

        return fileGenService.CreateEntryAsync(
            archive,
            fileName.ToDomainProjectFile(model.AppName),
            new DomainEntityModel
            {
                Namespace = $"{model.AppName}.Domain.Entities",
                EntityName = $"{model.EntityConfiguration.EntityName}Entity",
                Properties = mapper.Map<IEnumerable<DotnetPropertyModel>>(model.EntityConfiguration.Properties),
            },
            token);
    }
}
