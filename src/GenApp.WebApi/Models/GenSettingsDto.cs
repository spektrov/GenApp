using System.ComponentModel.DataAnnotations;
using GenApp.Domain.Enums;

namespace GenApp.WebApi.Models;

public class GenSettingsDto
{
    public string SqlScript { get; set; }

    public DbmsType DbmsType { get; set; }

    [MinLength(2)]
    required public string AppName { get; set; }

    public int DotnetSdkVersion { get; set; }

    public bool UseDocker { get; set; }
}