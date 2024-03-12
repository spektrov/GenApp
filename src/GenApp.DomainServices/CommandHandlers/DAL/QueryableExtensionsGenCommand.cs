using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class QueryableExtensionsGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
            archive,
            $"Extensions/QueryableExtensions.cs".ToDalProjectFile(model.AppName),
            new QueryableExtensionsModel
            {
                Namespace = $"{model.AppName}.DAL.Extensions",
                Usings = new[]
                {
                   "System.Linq.Expressions",
                   "Microsoft.EntityFrameworkCore",
                   $"{model.AppName}.DAL.Enums",
                   $"{model.AppName}.DAL.Interfaces",
                   $"{model.AppName}.DAL.Specifications",
                }.Order(),
            },
            token);
    }
}
