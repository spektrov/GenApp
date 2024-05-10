using System.Text.RegularExpressions;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;

namespace GenApp.DomainServices.Services;
public class ConnectionDetailsProvider : IConnectionDetailsProvider
{
    private ConnectionDetailsModel? connectionDetails;

    public ConnectionDetailsModel Get(ApplicationDataModel model)
    {
        return connectionDetails ??= new ConnectionDetailsModel
        {
            User = GetUser(model.DbmsType),
            Password = Guid.NewGuid().ToString(),
            DbName = GetDbName(model.AppName),
        };
    }

    private string GetDbName(string appName)
    {
        var noSpace = appName.Replace(" ", string.Empty);
        var underscoreUsage = Regex.Replace(noSpace, "[^a-zA-Z0-9]", "_");
        return underscoreUsage.ToLower();
    }

    private string GetUser(DbmsType dbmsType)
    {
        return dbmsType switch
        {
            DbmsType.POSTGRESQL => "postgres",
            DbmsType.MYSQL => "admin",
            DbmsType.MSSQLSERVER => "admin",
            _ => "admin",
        };
    }
}