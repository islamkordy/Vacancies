using Application.Features.User.TokenService.Abstract;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Persistance.UserMangement
{
    public class UserManager : IUserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        public UserManager(IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public bool CheckAuthorize()
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _httpContextAccessor.HttpContext?.User;
            var checkToken = IsTokenExpired(token);
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated && !checkToken)
            {
                return true;
            }
            return false;
        }

        private bool IsTokenExpired(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return true;
            }

            var now = DateTime.UtcNow;
            if (jwtToken.ValidTo < now)
            {
                return true;
            }

            return false;
        }

        public string GetEmail()
        {
            var userIdentity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;

            var Email = userIdentity!.FindFirst("emailRegister")!.Value;

            return Email;
        }

        public string GetRole()
        {
            var userIdentity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
            if (userIdentity.Claims.Count() > 0)
                return userIdentity.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            return null;
        }

        public string GetRole(string token)
        {
            var userClaims = _tokenService.ValidateAccessTokenString(token);

            return userClaims.Claims.First(x => x.Type == ClaimTypes.Role).Value;
        }

        public string GetUserId()
        {
            var userIdentity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;

            var UserId = userIdentity!.FindFirst(JwtRegisteredClaimNames.Sid)!.Value;

            return UserId;
        }

        public string GetUserId(string token)
        {
            var userClaims = _tokenService.ValidateAccessTokenString(token);

            var UserId = userClaims.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid)!.Value;

            return UserId;
        }
    }
}
