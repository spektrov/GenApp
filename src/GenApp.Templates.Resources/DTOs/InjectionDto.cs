namespace GenApp.Templates.Resources.DTOs;
public class InjectionDto
{
    public string InterfaceName { get; set; }

    public string ClassName { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is InjectionDto dto &&
            InterfaceName == dto.InterfaceName &&
            ClassName == dto.ClassName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
