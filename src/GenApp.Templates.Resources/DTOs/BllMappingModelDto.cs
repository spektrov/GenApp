namespace GenApp.Templates.Resources.DTOs;
public class BllMappingModelDto
{
    public string CommandModelName { get; set; }

    public string EntityName { get; set; }

    public string ModelName { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is BllMappingModelDto dto &&
            dto.CommandModelName == CommandModelName &&
            dto.EntityName == EntityName &&
            dto.ModelName == ModelName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
