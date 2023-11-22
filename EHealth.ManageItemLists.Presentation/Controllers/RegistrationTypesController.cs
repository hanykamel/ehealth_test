using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.Queries;
using EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.DTOs;
using EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RegistrationTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<RegistrationTypeDto>), 200)]
        public async Task<ActionResult<PagedResponse<RegistrationTypeDto>>> Search([FromQuery] RegistrationTypesSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
