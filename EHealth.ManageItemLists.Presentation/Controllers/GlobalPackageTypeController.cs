using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.Queries.Handlers;
using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.Queries;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalPackageTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GlobalPackageTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(GlobalPackageTypeDTO), 200)]
        public async Task<ActionResult<PagedResponse<GlobalPackageTypeDTO>>> Search([FromQuery]GlobalPackageTypeSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<GlobalPackageTypeDTO>> GetById([FromRoute] int id)
        {
            return Ok(await _mediator.Send(new GlobalPackageTypeGetByIdQuery(id)));
        }

    }
}
