using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Queries
{
    public class ResourceUHIAGetByIdQuery : IRequest<ResourceUHIAByIdDto>
    {
        public Guid Id { get; set; }
    }
}
