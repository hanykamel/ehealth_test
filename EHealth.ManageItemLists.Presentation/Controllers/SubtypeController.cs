using EHealth.ManageItemLists.Application.Lookups.Subtype.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Subtype.Queries;
using EHealth.ManageItemLists.Application.Lookups.Subtype.Queries.Handlers;
using EHealth.ManageItemLists.Application.Lookups.Type.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Type.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubtypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SubTypeDto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<SubTypeDto>> Get([FromRoute] int id)
        {
            return Ok(await _mediator.Send(new GetSubTypeByIdQuery(id)));
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<SubTypeDto>), 200)]
        public async Task<ActionResult<PagedResponse<SubTypeDto>>> Search([FromQuery] SubTypeSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

