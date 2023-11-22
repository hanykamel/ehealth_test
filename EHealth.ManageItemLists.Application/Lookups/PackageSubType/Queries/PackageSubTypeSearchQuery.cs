﻿using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageSubType.DTOS;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PackageSubType.Queries
{
    public class PackageSubTypeSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<PackageSubTypeDTO>>
    {

    }
}
