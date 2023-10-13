using Application.Features.User.TokenService.Abstract;
using Application.Features.User.TokenService.Dtos;
using Domain.Entities;
using Domain.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistance.Configurations;
using Persistence.IRepositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.User.TokenService.Service
{
    public class JwtTokenService : ITokenService
    {
    
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettingsConfigrations  _JwtSettingsConfigrations;
        private readonly IGenericRepository<UserAccount> _userAccountRepository;
        private readonly static HashSet<string> _revokedTokens = new HashSet<string>();


        public JwtTokenService(IRefreshTokenService refreshTokenRepository, 
                               IOptions<JwtSettingsConfigrations> jwtSettingsConfigrations, 
                               IGenericRepository<UserAccount> userAccountRepository)
        {

            _refreshTokenService = refreshTokenRepository;
            _JwtSettingsConfigrations = jwtSettingsConfigrations.Value;
            _userAccountRepository = userAccountRepository;
        }

        public async Task<(string accessToken, string refreshToken)> GenerateTokens(UserAccount userAccount)
        {
            var accessToken = await GenerateAccessToken(userAccount);
            var refreshToken = GenerateRefreshToken(userAccount.Id);

            await _refreshTokenService.SaveRefreshToken(refreshToken);

            return (accessToken, refreshToken.Token)!;
        }

        public SecurityToken ValidateAccessToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_JwtSettingsConfigrations.AccessTokenSecret);

            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer =_JwtSettingsConfigrations.Issuer,
                ValidateAudience = true,
                ValidAudience = _JwtSettingsConfigrations.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return validatedToken;
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var revoked = await _refreshTokenService.RevokeRefreshToken(refreshToken);
            return revoked;
        }
        public bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(token);

            return jwtToken.ValidTo < DateTime.Now;
        }

        public async Task<bool>IsRefreshTokenExpired(string Token)
        {
            var refreshToken = await _refreshTokenService.GetRefreshToken(Token);
            if (refreshToken==null)
                return false;
            return DateTime.Now > refreshToken.ExpirationDate;
        }
        public async Task<string> IsTokenAndRefreshTokenExpired(TokenDto tokenDto)
        {
            var RefreshToken = await _refreshTokenService.GetRefreshToken(tokenDto.refreshToken);
            var User = await _userAccountRepository.GetObj(x => x.Id == RefreshToken.UserId);
            if (RefreshToken != null)
            {
                if (!RefreshToken.IsRefreshTokenExpired())
                {
                    var jwtToken = await  GenerateAccessToken(User!);
                    return jwtToken;
                }
            }
            return string.Empty;
        }

        private async Task<string> GenerateAccessToken(UserAccount userAccount)
        {
            var roleName =  GetDataFromEnumExtension.GetEnumNameFromNumber<Roles>(userAccount.RoleId!.Value);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtSettingsConfigrations.AccessTokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sid,userAccount.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,userAccount.Email ??""),
                new Claim("role", roleName.ToString()??""),
            };

            var token = new JwtSecurityToken(
                _JwtSettingsConfigrations.Issuer,
                _JwtSettingsConfigrations.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(3600),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(Guid userId)
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = new RefreshToken();
                refreshToken.AddRefresh(Convert.ToBase64String(randomNumber), DateTime.Now.AddSeconds(18000), userId);
                return refreshToken;
            }
        }

        public ClaimsPrincipal ValidateAccessTokenString(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_JwtSettingsConfigrations.AccessTokenSecret);

           var claims= tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _JwtSettingsConfigrations.Issuer,
                ValidateAudience = true,
                ValidAudience = _JwtSettingsConfigrations.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return claims;
        }

        public bool IsTokenRevoked(string userId)
        {
            return _revokedTokens.Contains(userId);
        }

        public void RevokeToken(string userId)
        {
            _revokedTokens.Add(userId);
        }
    }
}