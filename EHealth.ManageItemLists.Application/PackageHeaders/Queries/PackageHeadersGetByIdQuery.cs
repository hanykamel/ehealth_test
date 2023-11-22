using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Queries
{
    public class PackageHeadersGetByIdQuery : IRequest<PackageHeadersGetByIdDTO>
    {
        public Guid Id { get; set; }
    }
}
