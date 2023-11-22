using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageComplexityClassificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PackageComplexityClassificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<PackageComplexityClassificationDto>), 200)]
        public async Task<ActionResult<PagedResponse<PackageComplexityClassificationDto>>> Search([FromQuery] PackageComplexityClassificationSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
