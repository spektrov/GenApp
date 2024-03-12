using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class EnumsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
            archive,
            $"Enums/SortingOrder.cs".ToDalProjectFile(model.AppName),
            new EnumSortingOrderModel
            {
                Namespace = $"{model.AppName}.DAL.Enums",
            },
            token);
    }
}
