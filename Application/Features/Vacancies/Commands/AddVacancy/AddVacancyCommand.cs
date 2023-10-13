using MediatR;

namespace Application.Features.Vacancies.Commands;

public class AddVacancyCommand : IRequest<ResponseModel<bool>>
{
    public string TitleEn { get; set; } = null!;
    public string TitleAr { get; set; } = null!;
    public string DescriptionEN { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
    public int MaxNumberOfApplicants { get; set; }
    public DateTime ExpiryDate { get; set; }
}
