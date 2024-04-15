using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class ExceptionsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
            archive,
            "Exceptions/NotFoundException.cs".ToBllProjectFile(model.AppName),
            new NotFoundExceptionModel
            {
                Namespace = $"{model.AppName}.BLL.Exceptions",
            },
            token);
    }
}
