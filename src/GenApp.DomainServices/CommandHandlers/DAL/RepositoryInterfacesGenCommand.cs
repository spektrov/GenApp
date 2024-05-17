using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class RepositoryInterfacesGenCommand(IFileGenService fileGenService, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await GenerateBaseRepositoryInterface(archive, model.AppName, token);

        foreach (var entity in model.Entities.AddIdFilter())
        {
            var name = caseTransformer.ToPascalCase(entity.EntityName);
            var entityName = $"{name}Entity";
            var fileName = $"I{name}Repository.cs";

            await fileGenService.CreateEntryAsync(
                archive,
                $"Interfaces/{fileName}".ToDalProjectFile(model.AppName),
                new RepositoryInterfaceModel
                {
                    Name = $"I{name}Repository",
                    EntityName = entityName,
                    KeyType = entity.IdType,
                    Namespace = $"{model.AppName}.DAL.Interfaces",
                    Usings = new[] { $"{model.AppName}.DAL.Entities" },
                },
                token);
        }
    }

    private Task GenerateBaseRepositoryInterface(ZipArchive archive, string appName, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
           archive,
           "Interfaces/IRepositoryBase.cs".ToDalProjectFile(appName),
           new RepositoryBaseInterfaceModel
           {
               Namespace = $"{appName}.DAL.Interfaces",
               Usings = new[]
               {
                   "System.Linq.Expressions",
                   "Microsoft.EntityFrameworkCore",
                   $"{appName}.DAL.Enums",
                   $"{appName}.DAL.Models",
                   $"{appName}.DAL.Specifications",
               }.Order(),
           },
           token);
    }
}
