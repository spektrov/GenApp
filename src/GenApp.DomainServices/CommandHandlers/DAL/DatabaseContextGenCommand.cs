using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class DatabaseContextGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var sets = model.Entities.Select(entity =>
            new DbSetDto
            {
                EntityName = $"{entity.EntityName}".ToEntityName().ToPascalCase(),
                CollectionName = entity.EntityName.ToPlural().ToPascalCase(),
            });

        return fileGenService.CreateEntryAsync(
            archive,
            $"DatabaseContext.cs".ToDalProjectFile(model.AppName),
            new DatabaseContextModel
            {
                Namespace = $"{model.AppName}.DAL",
                Usings = new[]
                {
                    "Microsoft.EntityFrameworkCore",
                    $"{model.AppName}.DAL.Entities",
                }.Order(),
                Sets = sets,
            },
            token);
    }
}
