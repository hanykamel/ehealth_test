using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.Queries;
using EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.Queries;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageSpecialtyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PackageSpecialtyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<PackageSpecialtyDto>), 200)]
        public async Task<ActionResult<PagedResponse<PackageSpecialtyDto>>> Search([FromQuery] PackagePackageSpecialtyQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
