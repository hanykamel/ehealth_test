using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.SubCategories.Queries
{
    public class SubCategoriesSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<SubCategoryDto>>
    {
        public int ItemListSubtypeId { get; set; }
    }
}
