using Application.Features.User.TokenService.Dtos;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Application.Features.User.TokenService.Abstract
{
    public interface ITokenService
    {
        Task<(string accessToken, string refreshToken)> GenerateTokens(UserAccount userAccount);
        SecurityToken ValidateAccessToken(string accessToken);
        ClaimsPrincipal ValidateAccessTokenString(string accessToken);
        Task<bool> RevokeRefreshToken(string refreshToken);
        Task<string> IsTokenAndRefreshTokenExpired(TokenDto tokenDto);
        Task<bool> IsRefreshTokenExpired(string Token);
        bool IsTokenExpired(string token);
        bool IsTokenRevoked(string token);
        void RevokeToken(string userId);
    }
}
