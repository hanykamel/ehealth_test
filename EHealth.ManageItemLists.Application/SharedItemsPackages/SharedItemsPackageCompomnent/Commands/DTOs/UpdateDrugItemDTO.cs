using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.DTOs
{
    public class UpdateDrugItemDTO
    {
        public Guid SharedItemsPackageDrugId { get; set; }
        public Guid PackageHeaderId { get; set; }
        public Guid DrugUHIAId { get; set; }
        public int Quantity { get; set; }
        public int NumberOfCasesInTheUnit { get; set; }
        public int? LocationId { get; set; }
        public double? TotalCost { get; set; }
        public double? DrugPerCase { get; set; }
    }
}
