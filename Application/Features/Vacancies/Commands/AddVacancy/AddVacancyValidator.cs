using FluentValidation;

namespace Application.Features.Vacancies.Commands;

internal class AddVacancyValidator : AbstractValidator<AddVacancyCommand>
{
    public AddVacancyValidator()
    {
        RuleFor(x => x.TitleAr)
            .NotEmpty();
        RuleFor(x => x.TitleEn)
            .NotEmpty();
        RuleFor(x => x.DescriptionAr)
            .NotEmpty();
        RuleFor(x => x.DescriptionEN)
            .NotEmpty();
    }
}
