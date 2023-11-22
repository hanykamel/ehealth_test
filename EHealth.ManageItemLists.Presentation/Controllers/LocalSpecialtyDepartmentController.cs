using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.Queries;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class LocalSpecialtyDepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LocalSpecialtyDepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<LocalSpecialtyDepartmentDto>), 200)]
        public async Task<ActionResult<PagedResponse<LocalSpecialtyDepartmentDto>>> Search([FromQuery] LocalSpecialtyDepartmentsSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
