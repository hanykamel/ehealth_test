using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitRooms.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitRooms.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitRoomsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UnitRoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<UnitRoomDto>), 200)]
        public async Task<ActionResult<PagedResponse<UnitRoomDto>>> Search([FromQuery] UnitRoomsSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
