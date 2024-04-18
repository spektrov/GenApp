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
        var applicationData = settingsModel;
        applicationData.AppName = caseTransformer.ToPascalCase(settingsModel.AppName);

        var sqlTableConfigs = sqlTableParser.BuildTablesConfiguration(settingsModel.SqlTableScript);
        if (sqlTableConfigs.IsFailed) return sqlTableConfigs.ToResult();

        var entities = dotnetEntityFactory.Create(sqlTableConfigs.Value, settingsModel.DbmsType);
        if (entities.IsFailed) return entities.ToResult();
        applicationData.Entities = entities.Value;

        return applicationData;
    }
}
