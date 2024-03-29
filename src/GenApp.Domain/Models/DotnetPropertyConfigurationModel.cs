﻿namespace GenApp.Domain.Models;
public class DotnetPropertyConfigurationModel
{
    required public string Name { get; set; }

    required public string Type { get; set; }

    public bool NotNull { get; set; }

    public bool IsId { get; set; }

    public bool IsNavigationKey { get; set; }

    public string? RelationToEntity { get; set; }
}
