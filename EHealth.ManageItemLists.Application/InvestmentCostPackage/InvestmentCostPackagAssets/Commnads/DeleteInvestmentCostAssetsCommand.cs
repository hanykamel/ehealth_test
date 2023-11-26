using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads
{
    public class DeleteInvestmentCostAssetsCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
