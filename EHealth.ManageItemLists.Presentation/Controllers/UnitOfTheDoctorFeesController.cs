using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfTheDoctorFeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UnitOfTheDoctorFeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<UnitOfTheDoctorFeesDto>), 200)]
        public async Task<ActionResult<PagedResponse<UnitOfTheDoctorFeesDto>>> Search([FromQuery] UnitOfTheDoctorFeesSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
