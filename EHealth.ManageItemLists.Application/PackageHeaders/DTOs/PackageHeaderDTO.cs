using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageSubType.DTOS;
using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.DTOs
{
    public class PackageHeaderDTO
    {
        public Guid Id { get; set; }
        public string? EHealthCode { get; set; }
        public string UHIACode { get; set; }
        public string NameEn { get; set; }
        public string? NameAr { get; set; }
        public int PackageTypeId { get; set; }
        public PackageTypeDto packageType { get; set; }
        public int PackageSubTypeId { get; set; }
        public PackageSubTypeDTO packageSubType { get; set; }
        public int? PackageComplexityClassificationId { get; set; }
        public PackageComplexityClassificationDto packageComplexityClassification { get; set; }
        public int GlobelPackageTypeId { get; set; }
        public GlobalPackageTypeDTO globalPackageType { get; set; }
        public int PackageSpecialtyId { get; set; }
        public PackageSpecialtyDto packageSpecialty { get; set; }
        public int PackageDuration { get; set; }
        public DateTimeOffset ActivationDateFrom { get; set; }
        public DateTimeOffset? ActivationDateTo { get; set; }
        public double PackagePrice { get; set; }
        public double PackageRoundPrice { get; set; }
        public bool IsDeleted { get; set; }

        public static PackageHeaderDTO FromPackageHeader(PackageHeader input) => new PackageHeaderDTO
        {
            Id = input.Id,
            EHealthCode = input.EHealthCode,
            UHIACode = input.UHIACode,
            NameEn = input.NameEn,
            NameAr = input.NameAr,
            PackageTypeId = input.PackageTypeId,
            packageType = PackageTypeDto.FromPackageType(input.PackageType),
            PackageSubTypeId = input.PackageSubTypeId,
            packageSubType = PackageSubTypeDTO.FromPackageSubType(input.PackageSubType),
            PackageComplexityClassificationId = input.PackageComplexityClassificationId,
            packageComplexityClassification = PackageComplexityClassificationDto.FromPackageComplexityClassification(input.PackageComplexityClassification),
            GlobelPackageTypeId = input.GlobelPackageTypeId,
            globalPackageType = GlobalPackageTypeDTO.FromGlobalPackageType(input.GlobelPackageType),
            PackageSpecialtyId = input.PackageSpecialtyId,
            packageSpecialty = PackageSpecialtyDto.FromPackageSpecialty(input.PackageSpecialty),
            PackageDuration = input.PackageDuration,
            ActivationDateFrom = input.ActivationDateFrom,
            ActivationDateTo = input.ActivationDateTo,
            PackagePrice = input.PackagePrice,
            PackageRoundPrice = input.PackageRoundPrice,
            IsDeleted = input.IsDeleted

        };

    }
}
