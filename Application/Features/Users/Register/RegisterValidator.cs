using Domain;
using FluentValidation;

namespace Application.Features.User.Register;

internal class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(x => x.ConfirmPassword)
            .Matches(Constants.password_regex);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Matches(Constants.password_regex);

        RuleFor(x => x.MobileNumber)
          .NotEmpty()
            .Matches(Constants.recipient_number_regex);

        RuleFor(x => x.RoleId)
        .IsInEnum();
    }

}
