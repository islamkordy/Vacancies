using Application.Features.User.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Login;

public class LoginCommand: IRequest<ResponseModel<LoginDto>>
{
    public string MobileNumber { get; set; }
    public string Password { get; set; }
    public Roles RoleId { get; set; }
}
