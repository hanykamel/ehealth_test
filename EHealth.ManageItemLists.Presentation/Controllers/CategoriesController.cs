using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<CategoryDto>), 200)]
        public async Task<ActionResult<PagedResponse<CategoryDto>>> Search([FromQuery] CategoriesSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
