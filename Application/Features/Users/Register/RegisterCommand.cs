using Domain.Entities;
using MediatR;

namespace Application.Features.User.Register;

public class RegisterCommand : IRequest<ResponseModel<Guid>>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public Roles RoleId { get; set; }
}
