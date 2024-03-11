namespace GenApp.Domain.Extensions;

public static class StringExtensions
{
    public static string ToPascalCase(this string str)
    {
        return !string.IsNullOrWhiteSpace(str)
            ? new string(str.Where(IsValid).Select((ch, i) => Transform(str, i)).ToArray())
            : str;
    }

    private static bool IsValid(char ch)
    {
        return char.IsLetter(ch);
    }

    private static char Transform(string str, int i)
    {
        return ShouldCapitalize(str, i)
                ? char.ToUpperInvariant(str[i])
                : char.ToLowerInvariant(str[i]);
    }

    private static bool ShouldCapitalize(string str, int i)
    {
        bool last = i == str.Length - 1;
        bool first = i == 0;
        bool isNextUpper = last || !last && char.IsUpper(str[i + 1]);
        bool isPreviousUpper = !first && char.IsUpper(str[i - 1]);
        bool isPreviousUnderscore = !first && str[i - 1] == '_';

        return char.IsUpper(str[i]) && !isPreviousUpper && !isNextUpper || isPreviousUnderscore;
    }
}