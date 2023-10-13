using Domain.Entities;

namespace Application.Features.User.TokenService.Abstract
{
    public interface IRefreshTokenService
    {
        Task<bool> SaveRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshToken(string token);
        Task<bool> RevokeRefreshToken(string token);

    }
}
