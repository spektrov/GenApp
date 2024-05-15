namespace GenApp.Templates.Resources.DTOs;
public class PackageDto
{
    public string Name { get; set; }

    public string Version { get; set; }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object? obj)
    {
        return obj is PackageDto dto
            && Name == dto.Name
            && Version == dto.Version;
    }
}
