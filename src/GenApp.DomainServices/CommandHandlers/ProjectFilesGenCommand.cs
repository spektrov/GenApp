using System.IO.Compression;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers;

internal class ProjectFilesGenCommand(IFileGenService fileGenService) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        var dllType = "Microsoft.NET.Sdk";
        var webType = "Microsoft.NET.Sdk.Web";

        await fileGenService.CreateEntryAsync(
            archive,
            $"{model.AppName}.DAL.csproj".ToDalProjectFile(model.AppName),
            new ProjectFileModel
            {
                Type = dllType,
                SdkVersion = model.DotnetSdkVersion,
                Packages = new List<PackageDto>
                {
                    new() { Name = "Microsoft.EntityFrameworkCore.Relational", Version = "6.0.27" },
                    DbPackageResolver(model.DbmsType),
                }
            },
            token);

        await fileGenService.CreateEntryAsync(
            archive,
            $"{model.AppName}.BLL.csproj".ToBllProjectFile(model.AppName),
            new ProjectFileModel
            {
                Type = dllType,
                SdkVersion = model.DotnetSdkVersion,
                Packages = new List<PackageDto>
                {
                    new() { Name = "AutoMapper", Version = "13.0.1" },
                },
                Includes = new List<string>
				{
					$"..\\{model.AppName}.DAL\\{model.AppName}.DAL.csproj"
				},
            },
            token);

        await fileGenService.CreateEntryAsync(
	        archive,
	        $"{model.AppName}.API.csproj".ToApiProjectFile(model.AppName),
	        new ProjectFileModel
	        {
		        Type = webType,
		        SdkVersion = model.DotnetSdkVersion,
		        Packages = new List<PackageDto>
		        {
					 new() { Name = "Swashbuckle.AspNetCore", Version = "6.5.0" },
					 new() { Name = "AutoMapper", Version = "13.0.1" },
		        },
		        Includes = new List<string>
                {
					$"..\\{model.AppName}.BLL\\{model.AppName}.BLL.csproj"
                },
	        },
	        token);
	}

    private PackageDto DbPackageResolver(DbmsType dbmsType)
    {
		return dbmsType switch
		{
			DbmsType.MSSQLSERVER => new PackageDto { Name = "Microsoft.EntityFrameworkCore.SqlServer", Version = "6.0.27" },
			DbmsType.MYSQL => new PackageDto { Name = "Pomelo.EntityFrameworkCore.MySql", Version = "6.0.2" },
			DbmsType.POSTGRESQL => new PackageDto { Name = "Npgsql.EntityFrameworkCore.PostgreSQL", Version = "6.0.22" },
			_ => throw new ArgumentException("Dbms type is not defined"),
		};
	}
}
