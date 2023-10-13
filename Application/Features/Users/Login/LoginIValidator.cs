using FluentValidation;

namespace Application.Features.Users.Login;

internal class LoginIValidator: AbstractValidator<LoginCommand>
{
    public LoginIValidator()
    {
        RuleFor(x => x.MobileNumber)
           .NotEmpty();

        RuleFor(x => x.RoleId)
        .IsInEnum();
    }
}
