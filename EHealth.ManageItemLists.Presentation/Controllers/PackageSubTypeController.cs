using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.Queries;
using EHealth.ManageItemLists.Application.Lookups.PackageSubType.DTOS;
using EHealth.ManageItemLists.Application.Lookups.PackageSubType.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageSubTypeController : ControllerBase
    {

        private readonly IMediator _mediator;
        public PackageSubTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<PackageSubTypeDTO>), 200)]
        public async Task<ActionResult<PagedResponse<PackageSubTypeDTO>>> Search([FromQuery] PackageSubTypeSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
