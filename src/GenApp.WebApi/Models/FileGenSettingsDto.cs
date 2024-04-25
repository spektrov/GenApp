namespace GenApp.WebApi.Models;

public class FileGenSettingsDto : GenSettingsDto
{
    required public IFormFile File { get; set; }
}
