using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.IRepositories;
using Application.Utils;

namespace Application.Features.User.Register;

internal class RegisterHandler : IRequestHandler<RegisterCommand, ResponseModel<Guid>>
{
    private readonly IValidator<RegisterCommand> _phoneNumberValidator;
    private readonly IGenericRepository<UserAccount> _userAccountRepo;

    public RegisterHandler(IValidator<RegisterCommand> phoneNumberValidator, IGenericRepository<UserAccount> userAccountRepo)
    { 
        _phoneNumberValidator = phoneNumberValidator;
        _userAccountRepo = userAccountRepo;
    }

    public async Task<ResponseModel<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var resultValidation = await _phoneNumberValidator.ValidateAsync(request);
        if (!resultValidation.IsValid)
        {
            return new ResponseModel<Guid> { Ok = false, Message = Helpers.ArrangeValidationErrors(resultValidation.Errors) };
        }

        var user = await _userAccountRepo.GetObj(x => x.MobileNumber == request.MobileNumber);
        if (user != null)
        {
            return new ResponseModel<Guid> { Ok = false};
        }

        var userAccount = new UserAccount();
        userAccount.HashingPassword(request.Password);
        userAccount.MobileNumber = request.MobileNumber;
        userAccount.FullName = request.FullName;
        userAccount.CreatedAt = DateTime.UtcNow;
        userAccount.Email = request.Email;
        userAccount.RoleId = (int)request.RoleId;

        await _userAccountRepo.Add(userAccount);
        await _userAccountRepo.Save();

        return new ResponseModel<Guid> { Ok = true, Data = userAccount!.Id, Message = "Success" };
    }
}
