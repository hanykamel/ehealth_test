using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitsTypes.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitsTypes.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UnitsTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<UnitsTypeDto>), 200)]
        public async Task<ActionResult<PagedResponse<UnitsTypeDto>>> Search([FromQuery] UnitsTypesSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
