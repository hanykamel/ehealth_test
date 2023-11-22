using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.DTOs
{
    public class PackageHeaderDTO
    {
        public string? EHealthCode { get; set; }
        public string UHIACode { get; set; }
        public string NameEn { get; set; }
        public string? NameAr { get; set; }
        public int PackageTypeId { get; set; }
        public int PackageSubTypeId { get; set; }
        public int? PackageComplexityClassificationId { get; set; }
        public int GlobelPackageTypeId { get; set; }
        public int PackageSpecialtyId { get; set; }
        public int PackageDuration { get; set; }
        public DateTimeOffset ActivationDateFrom { get; set; }
        public DateTimeOffset? ActivationDateTo { get; set; }
        public double PackagePrice { get; set; }
        public double PackageRoundPrice { get; set; }
        public bool IsDeleted { get; set; }

        public static PackageHeaderDTO FromPackageHeader(PackageHeader input) => new PackageHeaderDTO
        {
            EHealthCode = input.EHealthCode,
            UHIACode = input.UHIACode,
            NameEn = input.NameEn,
            NameAr = input.NameAr,
            PackageTypeId = input.PackageTypeId,
            PackageSubTypeId = input.PackageSubTypeId,
            PackageComplexityClassificationId = input.PackageComplexityClassificationId,
            GlobelPackageTypeId = input.GlobelPackageTypeId,
            PackageSpecialtyId = input.PackageSpecialtyId,
            PackageDuration = input.PackageDuration,
            ActivationDateFrom = input.ActivationDateFrom/*.ToString("yyyy-MM-dd")*/,
            ActivationDateTo = input.ActivationDateTo,
            PackagePrice = input.PackagePrice,
            PackageRoundPrice = input.PackageRoundPrice,
            IsDeleted = input.IsDeleted

        };

    }
}
