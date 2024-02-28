namespace GenApp.Domain.Models;
public class ApplicationDataModel : GenSettingsModel
{
    public IEnumerable<DotnetEntityConfigurationModel> Entities { get; set; }

    public IEnumerable<SqlTableConfigurationModel> Tables { get; set; }
}
