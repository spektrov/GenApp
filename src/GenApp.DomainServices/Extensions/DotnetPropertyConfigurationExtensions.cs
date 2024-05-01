using GenApp.Domain.Constants;
using GenApp.Domain.Models;

namespace GenApp.DomainServices.Extensions;
public static class DotnetPropertyConfigurationExtensions
{
    public static bool IsNavigationCollection(this DotnetPropertyConfigurationModel property)
    {
        return property.Relation != null
            && !property.Relation.IsOneToOne
            && property.Relation.IsReverted;
    }

    public static IEnumerable<DotnetPropertyConfigurationModel> AddSearchFilter(
        this IEnumerable<DotnetPropertyConfigurationModel> properties)
    {
        return properties.Where(x =>
            !x.IsId &&
            !x.IsForeignRelation &&
            DotnetFilterTypes.Search.Contains(x.Type));
    }

    public static IEnumerable<DotnetPropertyConfigurationModel> AddRangeFilter(
        this IEnumerable<DotnetPropertyConfigurationModel> properties)
    {
        return properties.Where(x =>
            !x.IsId &&
            !x.IsForeignRelation &&
            DotnetFilterTypes.Range.Contains(x.Type));
    }

    public static IEnumerable<DotnetPropertyConfigurationModel> AddFilter(
        this IEnumerable<DotnetPropertyConfigurationModel> properties)
    {
        return properties.Where(x =>
            !x.IsId &&
            DotnetFilterTypes.Filter.Contains(x.Type));
    }

    public static IEnumerable<DotnetPropertyConfigurationModel> AddSortFilter(
        this IEnumerable<DotnetPropertyConfigurationModel> properties)
    {
        return properties.Where(x =>
            !x.IsId &&
            !x.IsForeignRelation &&
            DotnetFilterTypes.Sort.Contains(x.Type));
    }
}
