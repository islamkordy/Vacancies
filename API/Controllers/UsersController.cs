using Application;
using Application.Features.User.Register;
using Application.Features.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<bool>))]
        [SwaggerOperation(Summary = "register")]
        public async Task<IActionResult> Register(RegisterCommand RegisterCommand)
        {
            var result = await _mediator.Send(RegisterCommand);
            return Ok(result);
        }
        

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<bool>))]
        [SwaggerOperation(Summary = "login")]
        public async Task<IActionResult> login(LoginCommand loginCommand)
        {
            var result = await _mediator.Send(loginCommand);
            return Ok(result);
        }

        //[HttpGet]
        //[Route("get-users")]
        //[ProducesResponseType(200, Type = typeof(ResponseModel<bool>))]
        //[ProducesResponseType(400, Type = typeof(ResponseModel<bool>))]
        //[Authorize]
        //public async Task<IActionResult> GetNonAdminUsers([FromQuery] UsersQuery UsersQuery)
        //{
        //    var result = await _mediator.Send(UsersQuery);
        //    return Ok(result);
        //}
    }
}
