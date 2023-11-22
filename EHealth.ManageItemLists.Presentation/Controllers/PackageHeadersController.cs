using EHealth.ManageItemLists.Application.PackageHeaders.Commands;
using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.PackageHeaders.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PackageHeadersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        public PackageHeadersController(IMediator mediator, IPackageHeaderRepository packageHeaderRepository)
        {
            _mediator = mediator;
            _packageHeaderRepository = packageHeaderRepository;
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<CreatedResult> Create([FromBody] CreatePackageHeaderDto request)
        {
            var id = await _mediator.Send(new CreatePackageHeaderCommand(request));
            return Created("api/PackageHeaders/" + id, id);
        }

        //[Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> Update([FromBody] UpdatePackageHeaderDto request)
        {
            bool res = await _mediator.Send(new UpdatePackageHeaderCommand(request, _packageHeaderRepository));
            return Ok(res);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<PackageHeaderDTO>),200)]
        public async Task<ActionResult<PagedResponse<PackageHeaderDTO>>> Search([FromQuery]PackageHeaderSearchQuery request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PackageHeadersGetByIdDTO), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<PackageHeadersGetByIdDTO>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new PackageHeadersGetByIdQuery { Id = id }));
        }
    }
}
