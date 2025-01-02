namespace GenApp.Parsers.Sql.Extensions;
public static class StringExtensions
{
    public static IEnumerable<string> SplitBySeparator(this string str, char separator)
    {
        return !string.IsNullOrWhiteSpace(str)
            ? str.Split(separator).Select(c => c.Trim())
            : Enumerable.Empty<string>();
    }

    public static string GetNameWithoutQuotes(this string str, bool keepCase = true)
    {
        if (ValueInQuotes(str))
        {
            return str[1..^1];
        }

        return keepCase ? str : str.ToLower();
    }

    public static bool ValueInQuotes(this string str)
    {
        return str.StartsWith(Constants.QuoteSeparator) && str.EndsWith(Constants.QuoteSeparator);
    }
}
