namespace GenApp.WebApi.Models;

public class StringGenSettingsDto : GenSettingsDto
{
    required public string SqlScript { get; set; }
}
