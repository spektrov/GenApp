namespace GenApp.DomainServices.Extensions;
public static class ModelNameExtensions
{
    public static string ToCsExtension(this string name)
    {
        return $"{name}.cs";
    }

    public static string ToEntityName(this string name)
    {
        return $"{name}Entity";
    }

    public static string ToDomainModelName(this string name)
    {
        return $"{name}Model";
    }

    public static string ToCommandModelName(this string name)
    {
        return $"{name}CommandModel";
    }

    public static string ToRequestName(this string name)
    {
        return $"{name}Request";
    }

    public static string ToResponseName(this string name)
    {
        return $"{name}Response";
    }

    public static string ToFilterParametersName(this string name)
    {
        return $"{name}FilterParameters";
    }

    public static string ToSearchParametersName(this string name)
    {
        return $"{name}SearchParameters";
    }

    public static string ToRangeParametersName(this string name)
    {
        return $"{name}RangeParameters";
    }

    public static string ToInterfaceName(this string name)
    {
        return $"I{name}";
    }
}
