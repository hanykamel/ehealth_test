using DocumentFormat.OpenXml.Wordprocessing;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.PackageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.DTOs
{
    public class CreatePackageHeaderDto
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
        public PackageHeader ToPackageHeader(string createdBy, string tenantId) => PackageHeader.Create(null, EHealthCode, UHIACode, NameEn, NameAr, PackageTypeId,
            PackageSubTypeId, PackageComplexityClassificationId, GlobelPackageTypeId, PackageSpecialtyId, PackageDuration,
            ActivationDateFrom, ActivationDateTo, PackagePrice, PackageRoundPrice, createdBy, tenantId);
    }
}
