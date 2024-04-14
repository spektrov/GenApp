using FluentResults;
using GenApp.Domain.Models;

namespace GenApp.Domain.Interfaces;
public interface IApplicationDataMapper
{
    Result<ApplicationDataModel> Map(ApplicationDataModel settingsModel);
}
