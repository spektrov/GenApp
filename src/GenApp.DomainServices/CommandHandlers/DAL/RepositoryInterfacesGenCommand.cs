using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class RepositoryInterfacesGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        foreach (var entity in model.Entities)
        {
            var name = entity.EntityName.ToPascalCase();
            var entityName = name.ToEntityName();
            var fileName = $"{name}{NameConstants.Repository}".ToInterfaceName().ToCsExtension();

            await fileGenService.CreateEntryAsync(
                archive,
                $"{NameConstants.Interfaces}/{fileName}".ToDalProjectFile(model.AppName),
                new RepositoryInterfaceModel
                {
                    Name = $"{name}{NameConstants.Repository}".ToInterfaceName(),
                    EntityName = entityName,
                    KeyType = entity.Properties.FirstOrDefault(x => x.IsId)?.Type ?? string.Empty,
                    Namespace = $"{model.AppName}.DAL.Interfaces",
                    Usings = new[] { $"{model.AppName}.DAL.Entities" },
                },
                token);
        }
    }
}
