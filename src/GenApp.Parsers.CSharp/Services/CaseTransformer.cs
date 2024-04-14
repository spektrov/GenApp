using GenApp.Parsers.Abstractions.Interfaces;
using Humanizer;

namespace GenApp.Parsers.CSharp.Services;

internal class CaseTransformer : ICaseTransformer
{
    /// <summary>
    /// Transforms string into camelCase.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>Transformed string.</returns>
    public string ToCamelCase(string str)
    {
        var result = ToPascalCase(str);
        return !string.IsNullOrWhiteSpace(result)
            ? char.ToLower(result[0]) + result[1..]
            : result;
    }

    /// <summary>
    /// Transforms string into _privateFieldCase.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>Transformed string.</returns>
    public string ToCamelUnderscoreCase(string str)
    {
        var result = ToPascalCase(str);
        return !string.IsNullOrWhiteSpace(result)
            ? "_" + char.ToLower(result[0]) + result[1..]
            : result;
    }

    /// <summary>
    /// Transforms string into snake_case.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>Transformed string.</returns>
    public string ToSnakeCase(string str)
    {
        var result = ToPascalCase(str);
        return !string.IsNullOrWhiteSpace(result)
            ? string.Concat(result.Select(TransformSnake))
            : result;
    }

    /// <summary>
    /// Transforms string into PascalCase.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>Transformed string.</returns>
    public string ToPascalCase(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return str;

        str = str.Equals(str.ToUpper()) ? str.ToLower() : str;

        var transformed = new string(str.Select((ch, i) => TransformPascal(str, i)).ToArray());
        return new string(transformed.Where(IsValid).ToArray());
    }

    /// <summary>
    /// Transforms string to it's English plural form.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>Transformed string.</returns>
    public string ToPlural(string str)
    {
        var words = ToSnakeCase(str).Split('_');
        var other = string.Join('_', words[..^1]);
        var last = words.Last().Pluralize();

        return !string.IsNullOrWhiteSpace(other)
            ? $"{other}_{last}"
            : last;
    }

    /// <summary>
    /// Transforms string to it's English singular form.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>Transformed string.</returns>
    public string ToSignular(string str)
    {
        var words = ToSnakeCase(str).Split('_');
        var other = string.Join('_', words[..^1]);
        var last = words.Last().Singularize();

        return !string.IsNullOrWhiteSpace(other)
            ? $"{other}_{last}"
            : last;
    }

    private static bool IsValid(char ch)
    {
        return char.IsLetter(ch);
    }

    private static string TransformSnake(char ch, int i)
    {
        var lower = ch.ToString().ToLowerInvariant();
        return i > 0 && char.IsUpper(ch) ? "_" + lower : lower;
    }

    private static char TransformPascal(string str, int i)
    {
        return ShouldCapitalize(str, i)
                ? char.ToUpperInvariant(str[i])
                : char.ToLowerInvariant(str[i]);
    }

    private static bool ShouldCapitalize(string str, int i)
    {
        return i == 0 || str[i - 1] == '_' || char.IsUpper(str[i]);
    }
}
