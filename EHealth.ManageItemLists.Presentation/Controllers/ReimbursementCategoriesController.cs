using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReimbursementCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReimbursementCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ReimbursementCategoryDto>), 200)]
        public async Task<ActionResult<PagedResponse<ReimbursementCategoryDto>>> Search([FromQuery] ReimbursementCategoriesSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
