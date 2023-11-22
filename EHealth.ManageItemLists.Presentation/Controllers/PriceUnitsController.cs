using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceUnitsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PriceUnitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<PriceUnitDto>), 200)]
        public async Task<ActionResult<PagedResponse<PriceUnitDto>>> Search([FromQuery] PriceUnitSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
