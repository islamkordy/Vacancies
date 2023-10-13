using Application;
using Application.Features.Vacancies.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VacanciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VacanciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("add-vacancy")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<bool>))]
        [SwaggerOperation(Summary = "addVacancy")]
        //[Authorize(Roles = nameof(Roles.Employer) )]
        [Authorize]
        public async Task<IActionResult> AddVacancy(AddVacancyCommand AddVacancyCommand)
        {
            var result = await _mediator.Send(AddVacancyCommand);
            return Ok(result);
        }
    }
}
