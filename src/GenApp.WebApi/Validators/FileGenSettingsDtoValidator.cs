using FluentValidation;
using GenApp.WebApi.Models;

namespace GenApp.WebApi.Validators;

public class FileGenSettingsDtoValidator : BaseGenSettingsDtoValidator<FileGenSettingsDto>
{
    private readonly IEnumerable<string> _validExtensions =
        [
            ".sql", ".txt"
        ];

    public FileGenSettingsDtoValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .Must(HaveValidExtension)
            .WithMessage($"Only {string.Join(", ", _validExtensions)} files are allowed.");
    }

    private bool HaveValidExtension(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _validExtensions.Contains(fileExtension);
    }
}
