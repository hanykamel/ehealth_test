using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.DTOs
{
    public class UpdatePackageHeaderDto
    {
        public Guid Id { get; set; }
        public string UHIACode { get; set; }
        public string NameEn { get; set; }
        public string? NameAr { get; set; }
        public int PackageTypeId { get; private set; }
        public int PackageSubTypeId { get; private set; }
        public int? PackageComplexityClassificationId { get; set; }
        public int GlobelPackageTypeId { get; set; }
        public int PackageSpecialtyId { get; set; }
        public int PackageDuration { get; set; }
        public DateTimeOffset ActivationDateFrom { get; set; }
        public DateTimeOffset? ActivationDateTo { get; set; }
        public double PackagePrice { get; set; }
        public double PackageRoundPrice { get; set; }
    }
}
