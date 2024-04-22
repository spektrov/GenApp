using System.IO.Compression;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class BootstrapGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var injections = model.Entities.Select(entity => new InjectionDto
        {
            InterfaceName = $"I{entity.EntityName}Repository",
            ClassName = $"{entity.EntityName}Repository",
        });

        await fileGenService.CreateEntryAsync(
            archive,
            $"Bootstrap.cs".ToDalProjectFile(model.AppName),
            new DalBootstrapModel
            {
                DbmsUsage = GetDbmsUsageMethod(model.DbmsType),
                Injections = injections,
                Namespace = $"{model.AppName}.DAL",
                Usings = new[]
                {
                    "Microsoft.EntityFrameworkCore",
                    "Microsoft.Extensions.Configuration",
                    "Microsoft.Extensions.DependencyInjection",
                    $"{model.AppName}.DAL.Interfaces",
                    $"{model.AppName}.DAL.Repositories",
                }.Order(),
            },
            token);
    }

    private string GetDbmsUsageMethod(DbmsType dbmsType)
    {
        return dbmsType switch
        {
            DbmsType.POSTGRESQL => "UseNpgsql",
            DbmsType.MYSQL => "UseMySql",
            DbmsType.MSSQLSERVER => "UseSqlServer",
            _ => throw new ArgumentException("Dbms type is not defined"),
        };
    }
}
