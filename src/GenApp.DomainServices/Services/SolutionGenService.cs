using FluentResults;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;

namespace GenApp.DomainServices.Services;

public class SolutionGenService(
    IApplicationDataMapper applicationDataMapper,
    IArchiveGenService archiveGenService,
    IEnumerable<IGenCommand> commands)
    : ISolutionGenService
{
    public async Task<Result<Stream>> GenerateApplicationAsync(GenSettingsModel settings, CancellationToken token)
    {
        var model = applicationDataMapper.Map(settings);
        if (model.IsFailed)
        {
            return model.ToResult();
        }

        using (var archive = archiveGenService.CreateArchive())
        {
            foreach (var command in commands)
            {
                await command.ExecuteAsync(archive, model.Value, token);
            }
        }

        archiveGenService.ResetPosition();

        return archiveGenService.MemoryStream;
    }
}