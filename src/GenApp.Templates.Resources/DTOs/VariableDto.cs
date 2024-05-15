namespace GenApp.Templates.Resources.DTOs;
public class VariableDto
{
    public string Name { get; set; }

    public string Value { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is VariableDto dto &&
            Name == dto.Name &&
            Value == dto.Value;
    }
}
