using FluentResults;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Extensions;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;

namespace GenApp.DomainServices.Mappers;
public class ApplicationDataModelMapper(ISqlTableParser sqlTableParser) : IApplicationDataMapper
{
    public Result<ApplicationDataModel> Map(GenSettingsModel settingsModel)
    {
        var applicationData = new ApplicationDataModel
        {
            AppName = settingsModel.AppName,
            DbmsType = settingsModel.DbmsType,
            SqlTableScript = settingsModel.SqlTableScript,
            DotnetSdkVersion = BuildNetSdkVersion(settingsModel.DotnetSdkVersion)
        };

        var sqlTableConfigs = sqlTableParser.BuildTablesConfiguration(settingsModel.SqlTableScript);
        if (sqlTableConfigs.IsFailed) return sqlTableConfigs.ToResult();
        applicationData.Tables = sqlTableConfigs.Value;

        applicationData.Entities = BuildDotnetEntities(sqlTableConfigs.Value, settingsModel.DbmsType);

        return applicationData;
    }

    private string BuildNetSdkVersion(string version)
    {
        return $"net{version}.0";
    }

    private IEnumerable<DotnetEntityConfigurationModel> BuildDotnetEntities(IEnumerable<SqlTableConfigurationModel> tables, DbmsType dbms)
    {
        return tables.Select(table => new DotnetEntityConfigurationModel
        {
            EntityName = table.TableName.ToPascalCase(),
            Properties = table.Columns
            .Select(column => new DotnetPropertyConfigurationModel
            {
                Name = !column.IsPrimaryKey ? column.ColumnName.ToPascalCase() : NameConstants.Id,
                Type = PropertyTypeMapper.Map(dbms, column.ColumnType),
                NotNull = column.NotNull,
                IsId = column.IsPrimaryKey,
            }),
        });
    }
}
