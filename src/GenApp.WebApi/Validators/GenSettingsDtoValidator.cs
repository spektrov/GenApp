using FluentValidation;
using GenApp.WebApi.Models;

namespace GenApp.WebApi.Validators;

public class GenSettingsDtoValidator : AbstractValidator<GenSettingsDto>
{
    public GenSettingsDtoValidator()
    {
        RuleFor(x => x.SqlScript).NotEmpty();
        RuleFor(x => x.DbmsType).NotEmpty();
        RuleFor(x => x.AppName).NotEmpty().Length(2, 30);
        RuleFor(x => x.DotnetSdkVersion).GreaterThanOrEqualTo(6).LessThanOrEqualTo(8);
        When(x => !x.UseDocker, () =>
        {
            RuleFor(x => x.ConnectionString).NotEmpty();
        });
    }
}
