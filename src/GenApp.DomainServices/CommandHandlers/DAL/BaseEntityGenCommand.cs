using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class BaseEntityGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var fileName = "IBaseEntity.cs";
        var dirName = "Interfaces";

        return fileGenService.CreateEntryAsync(
            archive,
            $"{dirName}/{fileName}".ToDalProjectFile(model.AppName),
            new EntityBaseModel
            {
                Namespace = $"{model.AppName}.DAL.{dirName}",
            },
            token);
    }
}
