using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries
{
    public class InvestmentCostPackageFacilityGetByIdQuery : IRequest<InvestmentCostPackageAssetsDTO>
    {
        public Guid? FacilityId { get; set; }

    }
}
