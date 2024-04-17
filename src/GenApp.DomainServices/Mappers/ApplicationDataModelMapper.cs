using FluentResults;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.Parsers.Abstractions.Interfaces;

namespace GenApp.DomainServices.Mappers;

public class ApplicationDataModelMapper(
    ISqlTableParser sqlTableParser,
    ICaseTransformer caseTransformer,
    IDotnetEntityFactory dotnetEntityFactory)
    : IApplicationDataMapper
{
    public Result<ApplicationDataModel> Map(ApplicationDataModel settingsModel)
    {
        var applicationData = new ApplicationDataModel
        {
            AppName = caseTransformer.ToPascalCase(settingsModel.AppName),
            DbmsType = settingsModel.DbmsType,
            SqlTableScript = settingsModel.SqlTableScript,
            DotnetSdkVersion = BuildNetSdkVersion(settingsModel.DotnetSdkVersion),
            UseDocker = settingsModel.UseDocker,
        };

        var sqlTableConfigs = sqlTableParser.BuildTablesConfiguration(settingsModel.SqlTableScript);
        if (sqlTableConfigs.IsFailed) return sqlTableConfigs.ToResult();

        var entities = dotnetEntityFactory.Create(sqlTableConfigs.Value, settingsModel.DbmsType);
        if (entities.IsFailed) return entities.ToResult();
        applicationData.Entities = entities.Value;

        return applicationData;
    }

    private string BuildNetSdkVersion(string version)
    {
        return $"net{version}.0";
    }
}
