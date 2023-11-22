using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.Categories.Queries
{
    public class CategoriesSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<CategoryDto>>
    {
        public int ItemListSubtypeId { get; set; }
    }
}
