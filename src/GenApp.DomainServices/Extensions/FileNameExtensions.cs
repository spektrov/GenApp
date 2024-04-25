namespace GenApp.DomainServices.Extensions;

public static class FileNameExtensions
{
    public static string ToCoreSolutionFile(this string filename)
    {
        return $"{filename}";
    }

    public static string ToDbFile(this string filename)
    {
        return $"db/{filename}";
    }

    public static string ToDalProjectFile(this string filename, string appName)
    {
        return $"src/{appName}.DAL/{filename}";
    }

    public static string ToApiProjectFile(this string filename, string appName)
    {
        return $"src/{appName}.API/{filename}";
    }

    public static string ToBllProjectFile(this string filename, string appName)
    {
        return $"src/{appName}.BLL/{filename}";
    }
}
