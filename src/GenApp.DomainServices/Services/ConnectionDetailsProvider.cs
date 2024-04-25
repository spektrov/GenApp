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
            User = "admin",
            Password = Guid.NewGuid().ToString(),
            DbName = model.AppName.ToLower()
        };
    }
}