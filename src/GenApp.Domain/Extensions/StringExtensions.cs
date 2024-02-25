using System.Globalization;
using System.Text.RegularExpressions;

namespace GenApp.Domain.Extensions;

public static class StringExtensions
{
    public static string ToPascalCase(this string str)
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        str = textInfo.ToTitleCase(str).Replace("_", string.Empty);
        return Regex.Replace(str, @"\s+", string.Empty);
    }
}