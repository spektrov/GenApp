using GenApp.Domain.Enums;

namespace GenApp.Domain.Models;

public class GenSettingsModel
{
    public DbmsType DbmsType { get; set; }

    public string SqlTableScript { get; set; }

    public string AppName { get; set; }

    public string DotnetSdkVersion { get; set; }

    public bool UseDocker { get; set; }

    public string DalProjectName { get; set; }

    public string BllProjectName { get; set; }

    public string ApiProjectName { get; set; }
}
