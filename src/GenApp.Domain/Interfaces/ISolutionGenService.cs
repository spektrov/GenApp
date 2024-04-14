using FluentResults;
using GenApp.Domain.Models;

namespace GenApp.Domain.Interfaces;

public interface ISolutionGenService
{
    Task<Result<Stream>> GenerateApplicationAsync(ApplicationDataModel settings, CancellationToken token);
}