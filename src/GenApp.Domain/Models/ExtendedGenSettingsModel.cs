namespace GenApp.Domain.Models;
public class ExtendedGenSettingsModel : GenSettingsModel
{
    public DotnetEntityConfigurationModel EntityConfiguration { get; set; }

    public SqlTableConfigurationModel TableConfiguration { get; set; }
}
