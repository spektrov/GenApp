using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class BaseRepositoryGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var fileName = "IRepositoryBase.cs";
        var dirName = "Interfaces";

        await fileGenService.CreateEntryAsync(
           archive,
           $"{dirName}/{fileName}".ToDalProjectFile(model.AppName),
           new RepositoryBaseInterfaceModel
           {
               Namespace = $"{model.AppName}.DAL.{dirName}",
               Usings = new[]
               {
                   "System.Linq.Expressions",
                   "Microsoft.EntityFrameworkCore",
                   $"{model.AppName}.DAL.Enums",
                   $"{model.AppName}.DAL.Models",
                   $"{model.AppName}.DAL.Specifications",
               }.Order(),
           },
           token);

        await fileGenService.CreateEntryAsync(
           archive,
           "Repositories/RepositoryBase.cs".ToDalProjectFile(model.AppName),
           new RepositoryBaseInterfaceModel
           {
               Namespace = $"{model.AppName}.DAL.Repositories",
               Usings = new[]
               {
                   "System.Linq.Expressions",
                   "Microsoft.EntityFrameworkCore",
                   "System.Text.Json",
                   $"{model.AppName}.DAL.Enums",
                   $"{model.AppName}.DAL.Extensions",
                   $"{model.AppName}.DAL.Extensions",
                   $"{model.AppName}.DAL.Interfaces",
                   $"{model.AppName}.DAL.Specifications",
                   $"{model.AppName}.DAL.Specifications.Orerators",
               }.Order(),
           },
           token);
    }
}
