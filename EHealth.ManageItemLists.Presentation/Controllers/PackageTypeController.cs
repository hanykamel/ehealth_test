using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageType.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PackageTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<PackageTypeDto>), 200)]
        public async Task<ActionResult<PagedResponse<PackageTypeDto>>> Search([FromQuery] GetPackageTypesQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
