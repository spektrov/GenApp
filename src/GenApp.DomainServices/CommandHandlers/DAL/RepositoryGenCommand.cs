﻿using System.IO.Compression;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Extensions;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.CommandHandlers.DAL;
internal class RepositoryGenCommand(IFileGenService fileGenService, ICaseTransformer caseTransformer) : IGenCommand
{
    public async Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token)
    {
        await GenerateBaseRepositoryClass(archive, model.AppName, token);

        foreach (var entity in model.Entities.AddIdFilter())
        {
            await GenerateRepository(archive, entity, model.AppName, token);
        }
    }

    private Task GenerateRepository(ZipArchive archive, DotnetEntityConfigurationModel entity, string appName, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
               archive,
               $"Repositories/{entity.EntityName}Repository.cs".ToDalProjectFile(appName),
               new EntityRepositoryModel
               {
                   Namespace = $"{appName}.DAL.Repositories",
                   RepositoryName = $"{entity.EntityName}Repository",
                   InterfaceName = $"I{entity.EntityName}Repository",
                   EntityName = $"{entity.EntityName}Entity",
                   KeyType = entity.IdType,
                   FilterParametersName = $"{entity.EntityName}FilterParameters",
                   SearchParametersName = $"{entity.EntityName}SearchParameters",
                   RangeParametersName = $"{entity.EntityName}RangeParameters",
                   FilterParameters = GetFilterParameters(entity),
                   SearchParameters = GetSearchParameters(entity),
                   RangeParameters = GetRangeParameters(entity),
                   SortParameters = GetSortParameters(entity),
                   Usings = new[]
                   {
                       "System.Linq.Expressions",
                       "System.Text.Json",
                       $"{appName}.DAL.Entities",
                       $"{appName}.DAL.Interfaces",
                       $"{appName}.DAL.Models",
                       $"{appName}.DAL.Specifications",
                       $"{appName}.DAL.Models.{entity.EntityName}Models",
                       $"{appName}.DAL.Specifications.{entity.EntityName}Specifications",
                   }.Order(),
               },
               token);
    }

    private Task GenerateBaseRepositoryClass(ZipArchive archive, string appName, CancellationToken token)
    {
        return fileGenService.CreateEntryAsync(
           archive,
           "Repositories/RepositoryBase.cs".ToDalProjectFile(appName),
           new RepositoryBaseModel
           {
               Namespace = $"{appName}.DAL.Repositories",
               Usings = new[]
               {
                   "System.Linq.Expressions",
                   "System.Text.Json",
                   "Microsoft.EntityFrameworkCore",
                   $"{appName}.DAL.Enums",
                   $"{appName}.DAL.Extensions",
                   $"{appName}.DAL.Interfaces",
                   $"{appName}.DAL.Models",
                   $"{appName}.DAL.Specifications",
                   $"{appName}.DAL.Specifications.Orerators",
               }.Order(),
           },
           token);
    }

    private ICollection<RepositoryFilterDto> GetFilterParameters(DotnetEntityConfigurationModel entity)
    {
        var filterDtos = new List<RepositoryFilterDto>();
        var filterProperties = entity.Properties.AddFilter();
        if (filterProperties.Any())
        {
            filterDtos = filterProperties.Select(x => new RepositoryFilterDto
            {
                PropertyName = caseTransformer.ToPascalCase(caseTransformer.ToPlural(x.Name)),
                SpecificationName = $"Find{entity.EntityName}By{x.Name}",
            }).ToList();
        }

        return filterDtos;
    }

    private ICollection<RepositoryFilterDto> GetSearchParameters(DotnetEntityConfigurationModel entity)
    {
        var searchDtos = new List<RepositoryFilterDto>();
        var searchProperties = entity.Properties.AddSearchFilter();
        if (searchProperties.Any())
        {
            searchDtos = searchProperties.Select(x => new RepositoryFilterDto
            {
                PropertyName = x.Name,
                SpecificationName = $"Search{entity.EntityName}By{x.Name}",
            }).ToList();
        }

        return searchDtos;
    }

    private ICollection<RepositoryFilterDto> GetRangeParameters(DotnetEntityConfigurationModel entity)
    {
        var rangeDtos = new List<RepositoryFilterDto>();
        var rangeProperties = entity.Properties.AddRangeFilter();
        if (rangeProperties.Any())
        {
            rangeDtos = rangeProperties.Select(x => new RepositoryFilterDto
            {
                PropertyName = x.Name,
                SpecificationName = $"Range{entity.EntityName}By{x.Name}",
            }).ToList();
        }

        return rangeDtos;
    }

    private ICollection<SortPropertyDto> GetSortParameters(DotnetEntityConfigurationModel entity)
    {
        var sortProperties = entity.Properties.AddSortFilter();
        var sortDtos = sortProperties.Select(x => new SortPropertyDto
        {
            SortName = x.Name,
            JsonName = caseTransformer.ToSnakeCase(x.Name),
        }).ToList();

        return sortDtos;
    }
}
