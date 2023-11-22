using EHealth.ManageItemLists.Application.Lookups.Subtype.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.Subtype.Queries
{
    public class SubTypeSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<SubTypeDto>>
    {

    }
}
