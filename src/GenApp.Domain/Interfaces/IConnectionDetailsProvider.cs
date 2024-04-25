using GenApp.Domain.Models;

namespace GenApp.Domain.Interfaces;
public interface IConnectionDetailsProvider
{
    ConnectionDetailsModel Get(ApplicationDataModel model);
}
