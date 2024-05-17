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
        if (string.IsNullOrEmpty(settingsModel.SqlTableScript))
        {
            return Result.Fail("SQL script is not provided");
        }

        var applicationData = settingsModel;
        applicationData.AppName = GetAppName(settingsModel.AppName);

        var sqlTableConfigs = sqlTableParser.BuildTablesConfiguration(settingsModel.SqlTableScript);
        if (sqlTableConfigs.IsFailed) return sqlTableConfigs.ToResult();

        var entities = dotnetEntityFactory.Create(sqlTableConfigs.Value, settingsModel.DbmsType);
        if (entities.IsFailed) return entities.ToResult();
        applicationData.Entities = entities.Value;

        return applicationData;
    }

    private string GetAppName(string appName)
    {
        var noSpace = appName.Replace(" ", string.Empty);
        return caseTransformer.ToPascalCase(noSpace);
    }
}
