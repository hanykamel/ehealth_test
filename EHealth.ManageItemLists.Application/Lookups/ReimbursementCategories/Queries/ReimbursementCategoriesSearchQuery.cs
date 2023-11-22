using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.Queries
{
    public class ReimbursementCategoriesSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<ReimbursementCategoryDto>>
    {
    }
}
