using System.ComponentModel.DataAnnotations;
using GenApp.Domain.Enums;

namespace GenApp.WebApi.Models;

public class GenSettingsDto
{
    required public string SqlScript { get; set; }

    required public DbmsType DbmsType { get; set; }

    required public string AppName { get; set; }

    public int DotnetSdkVersion { get; set; }

    public bool UseDocker { get; set; }

    public string? ConnectionString { get; set; }
}