//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using Swashbuckle.AspNetCore.Annotations;
//using Vacancies.Application.Features.UserManagement.TokenService.Abstract;
//using Vacancies.Application.Features.UserManagement.TokenService.Service;
//using Vacancies.Application.Models;
//using Vacancies.Infrastructure.TokenService.Dtos;

//namespace Vacancies.API.Controllers
//{
//    [Route("api/v1/[controller]")]
//    [ApiController]
//    public class TokensController : ControllerBase
//    {
//        private readonly ITokenService _tokenService;
//        public TokensController(ITokenService tokenService)
//        {
//            _tokenService = tokenService;
//        }
//        [HttpPost]
//        [Route("RefreshToken")]
//        [ProducesResponseType(200, Type = typeof(ResponseModel<bool>))]
//        [ProducesResponseType(400, Type = typeof(ResponseModel<bool>))]
//        [SwaggerOperation(Summary = "Generte User")]
//        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
//        {
//            var result = await _tokenService.IsTokenAndRefreshTokenExpired(tokenDto);
//            if (result.IsNullOrEmpty())
//            {
//                return Unauthorized();
//            }

//            return Ok(result);
//        }
//    }
//}
