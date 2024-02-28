using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers;
internal class BaseEntityGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var fileName = $"Interfaces/IBaseEntity.cs";
        return fileGenService.CreateEntryAsync(
            archive,
            fileName.ToDomainProjectFile(model.AppName),
            new BaseEntityModel
            {
                Namespace = $"{model.AppName}.Domain.Interfaces",
            },
            token);
    }
}
