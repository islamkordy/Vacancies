using Application.Features.User.TokenService.Abstract;
using Domain.Entities;
using Persistence.IRepositories;

namespace Application.Features.User.TokenService.Service
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IGenericRepository<RefreshToken> _RefreshTokenRepo;

        public RefreshTokenService(IGenericRepository<RefreshToken> refreshTokenRepo)
        {
            _RefreshTokenRepo = refreshTokenRepo;
        }

        public async Task<bool> SaveRefreshToken(RefreshToken refreshToken)
        {

            await _RefreshTokenRepo.Add(refreshToken);
            await _RefreshTokenRepo.Save();
            return true;
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            return (await _RefreshTokenRepo.GetObj(rt => rt.Token == token))!;
        }

        public async Task<bool> RevokeRefreshToken(string token)
        {
            var refreshToken = await _RefreshTokenRepo.GetObj(rt => rt.Token == token);
            if (refreshToken != null)
            {
                refreshToken.RevokeRefresh();
                await _RefreshTokenRepo.Save();
                return true;
            }
            return false;
        }
    }

}
