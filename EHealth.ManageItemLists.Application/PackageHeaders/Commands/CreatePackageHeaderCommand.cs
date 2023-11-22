using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands
{
    public record CreatePackageHeaderCommand(CreatePackageHeaderDto CreatePackageHeaderDto) : IRequest<Guid>;
}
