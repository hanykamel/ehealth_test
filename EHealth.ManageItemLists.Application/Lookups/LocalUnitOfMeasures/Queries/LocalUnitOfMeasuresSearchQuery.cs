using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.Queries
{
    public class LocalUnitOfMeasuresSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<UnitOfMeasureDto>>
    {
    }
}
