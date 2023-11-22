using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.DTOs;
using EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsPackageTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DrugsPackageTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<DrugsPackageTypesDto>), 200)]
        public async Task<ActionResult<PagedResponse<DrugsPackageTypesDto>>> Search([FromQuery] DrugsPackageTypesSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
