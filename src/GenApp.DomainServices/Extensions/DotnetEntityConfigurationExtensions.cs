using GenApp.Domain.Models;

namespace GenApp.DomainServices.Extensions;
public static class DotnetEntityConfigurationExtensions
{
    public static IEnumerable<DotnetEntityConfigurationModel> AddIdFilter(
        this IEnumerable<DotnetEntityConfigurationModel> entities)
    {
        return entities.Where(x => x.HasId);
    }

    public static bool HasId(this IEnumerable<DotnetEntityConfigurationModel> entities, string? entityName)
    {
        var entity = entities.FirstOrDefault(x =>
            string.Equals(x.EntityName, entityName, StringComparison.OrdinalIgnoreCase));

        return entity is { HasId: true };
    }
}
