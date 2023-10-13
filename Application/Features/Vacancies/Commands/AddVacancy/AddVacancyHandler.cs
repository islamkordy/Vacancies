using Application.Utils;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistance.UserMangement;
using Persistence.IRepositories;

namespace Application.Features.Vacancies.Commands;

internal class AddVacancyHandler : IRequestHandler<AddVacancyCommand, ResponseModel<bool>>
{
    private readonly IValidator<AddVacancyCommand> _phoneNumberValidator;
    private readonly IGenericRepository<UserAccount> _userAccountRepo;
    private readonly IGenericRepository<Vacancy> _vacancyRepo;
    private readonly IUserManager _userManager;

    public AddVacancyHandler(IValidator<AddVacancyCommand> phoneNumberValidator,
        IGenericRepository<UserAccount> userAccountRepo,
        IGenericRepository<Vacancy> vacancyRepo,
        IUserManager userManager)
    {
        _phoneNumberValidator = phoneNumberValidator;
        _userAccountRepo = userAccountRepo;
        _vacancyRepo = vacancyRepo;
        _userManager = userManager;
    }

    public async Task<ResponseModel<bool>> Handle(AddVacancyCommand request, CancellationToken cancellationToken)
    {
        var resultValidation = await _phoneNumberValidator.ValidateAsync(request);
        if (!resultValidation.IsValid)
            return new ResponseModel<bool> { Ok = false, Message = Helpers.ArrangeValidationErrors(resultValidation.Errors) };

        var userId = Guid.Parse(_userManager.GetUserId());
        var userAccount = await _userAccountRepo.GetObj(x => x.Id == userId);
        if (userAccount == null)
            return new ResponseModel<bool> { Ok = false, Message = "AccountNotFound"};
        if (userAccount.RoleId == (int)Roles.Employer)
            return new ResponseModel<bool> { Ok = false, Message = "AccountNotValid"};
        
        Vacancy vacancy = new Vacancy();
        vacancy.TitleAr = request.TitleAr;
        vacancy.TitleEn = request.TitleEn;
        vacancy.DescriptionAr = request.DescriptionAr;
        vacancy.DescriptionEN = request.DescriptionEN;
        vacancy.Status = true;
        vacancy.ExpiryDate = request.ExpiryDate;
        vacancy.MaxNumberOfApplicants = request.MaxNumberOfApplicants;

        await _vacancyRepo.Add(vacancy);
        await _vacancyRepo.Save();

        return new ResponseModel<bool> { Ok = true, Data = true, Message = "Success" };
    }
}
