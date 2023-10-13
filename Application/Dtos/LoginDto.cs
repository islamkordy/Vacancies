namespace Application.Features.User.DTO;

internal class LoginDto
{
    public string? Email { get; set; }
    public string? PwdHash { get; set; }
    public string? PwdSalt { get; set; }
    public int? RoleId { get; set; }
    public string? FullName { get; set; }
    public string? MobileNumber { get; set; }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
