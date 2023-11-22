using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.Queries
{
    public record GlobalPackageTypeGetByIdQuery(int id): IRequest<GlobalPackageTypeDTO>;
}
