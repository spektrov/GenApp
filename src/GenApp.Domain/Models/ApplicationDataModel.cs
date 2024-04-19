using GenApp.Domain.Enums;

namespace GenApp.Domain.Models;
public class ApplicationDataModel
{
    public DbmsType DbmsType { get; set; }

    public string SqlTableScript { get; set; }

    public string AppName { get; set; }

    public int DotnetSdkVersion { get; set; }

    public bool UseDocker { get; set; }

    public IEnumerable<DotnetEntityConfigurationModel> Entities { get; set; }
}
