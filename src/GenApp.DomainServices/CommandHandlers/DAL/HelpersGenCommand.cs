using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class HelpersGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await fileGenService.CreateEntryAsync(
            archive,
            $"Helpers/PageCountCalculator.cs".ToDalProjectFile(model.AppName),
            new PageCountCalcucatorModel
            {
                Namespace = $"{model.AppName}.DAL.Helpers",
                Usings = Array.Empty<string>(),
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            $"Helpers/SortingOrderCalculator.cs".ToDalProjectFile(model.AppName),
            new SortingOrderCalculatorModel
            {
                Namespace = $"{model.AppName}.DAL.Helpers",
                Usings = new[] { $"{model.AppName}.DAL.Enums" },
            },
            token);
    }
}
