using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageSummaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs
{
    public class InvestmentCostCalculateSummaryDto
    {
        public Guid PackageHeaderId { get; set; }
        public DateTime? PriceDate { get; set; }
    }
}
