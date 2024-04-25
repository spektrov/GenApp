using FluentValidation;
using GenApp.WebApi.Models;

namespace GenApp.WebApi.Validators;

public class StringGenSettingsDtoValidator : BaseGenSettingsDtoValidator<StringGenSettingsDto>
{
    public StringGenSettingsDtoValidator()
    {
        RuleFor(x => x.SqlScript).NotEmpty();
    }
}
