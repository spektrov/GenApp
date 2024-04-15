using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.BLL;
internal class StaticModelsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            "Models/QueryParameters.cs".ToBllProjectFile(model.AppName),
            new QueryParametersModel
            {
                Namespace = $"{model.AppName}.BLL.Models",
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            "Models/PaginationParameters.cs".ToBllProjectFile(model.AppName),
            new PaginationParametersModel
            {
                Namespace = $"{model.AppName}.BLL.Models",
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            "Models/PagedResponse.cs".ToBllProjectFile(model.AppName),
            new PagedResponseModel
            {
                Namespace = $"{model.AppName}.BLL.Models",
            },
            token);
    }
}
