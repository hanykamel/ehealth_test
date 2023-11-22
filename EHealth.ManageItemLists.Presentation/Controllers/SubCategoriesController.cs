using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<SubCategoryDto>), 200)]
        public async Task<ActionResult<PagedResponse<SubCategoryDto>>> Search([FromQuery] SubCategoriesSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
