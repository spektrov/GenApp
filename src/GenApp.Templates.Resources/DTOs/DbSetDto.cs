namespace GenApp.Templates.Resources.DTOs;
public class DbSetDto
{
    public string EntityName { get; set; }

    public string CollectionName { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is DbSetDto dto &&
            EntityName == dto.EntityName &&
            CollectionName == dto.CollectionName;
    }
}
