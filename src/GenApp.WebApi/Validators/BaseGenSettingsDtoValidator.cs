using FluentValidation;
using GenApp.WebApi.Models;

namespace GenApp.WebApi.Validators;

public class BaseGenSettingsDtoValidator<T> : AbstractValidator<T>
    where T : GenSettingsDto
{
    public BaseGenSettingsDtoValidator()
    {
        RuleFor(x => x.DbmsType)
            .NotEmpty();
        RuleFor(x => x.AppName)
            .NotEmpty()
            .Length(2, 30)
            .Matches(@"^[a-zA-Z]+$")
            .WithMessage("AppName can only contain Latin characters.");
        RuleFor(x => x.DotnetSdkVersion)
            .GreaterThanOrEqualTo(6)
            .LessThanOrEqualTo(8);
        When(x => !x.UseDocker, () =>
        {
            RuleFor(x => x.ConnectionString).NotEmpty();
        });
    }
}
