using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageSubType.DTOS;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.DTOs;

namespace EHealth.ManageItemLists.Application.PackageHeaders.DTOs
{
    public class PackageHeadersGetByIdDTO
    {
        public Guid Id { get; set; }
        public string? EHealthCode { get; set; }
        public string UHIACode { get; set; }
        public string NameEn { get; set; }
        public string? NameAr { get; set; }
        public int PackageTypeId { get; set; }
        public PackageTypeDto PackageType { get; set; }
        public int PackageSubTypeId { get; set; }
        public PackageSubTypeDTO PackageSubType { get; set; }
        public int? PackageComplexityClassificationId { get; set; }
        public PackageComplexityClassificationDto? PackageComplexityClassification { get; set; }
        public int GlobelPackageTypeId { get; set; }
        public GlobalPackageTypeDTO GlobelPackageType { get; set; }
        public int PackageSpecialtyId { get; set; }
        public PackageSpecialtyDto PackageSpecialty { get; set; }
        public int PackageDuration { get; set; }
        public DateTimeOffset ActivationDateFrom { get; set; }
        public DateTimeOffset? ActivationDateTo { get; set; }
        public double PackagePrice { get; set; }
        public double PackageRoundPrice { get; set; }
        public bool IsDeleted { get; set; }
        public static PackageHeadersGetByIdDTO FromPackageHeader(PackageHeader input) =>
        new PackageHeadersGetByIdDTO
        {
            Id = input.Id,
            EHealthCode = input.EHealthCode,
            UHIACode = input.UHIACode,
            NameEn = input.NameEn,
            NameAr = input.NameAr,
            PackageTypeId = input.PackageTypeId,
            PackageType = PackageTypeDto.FromPackageType(input.PackageType),
            PackageSubTypeId = input.PackageSubTypeId,
            PackageSubType = PackageSubTypeDTO.FromPackageSubType(input.PackageSubType),
            PackageComplexityClassificationId = input.PackageComplexityClassificationId,
            PackageComplexityClassification = PackageComplexityClassificationDto.FromPackageComplexityClassification(input.PackageComplexityClassification),
            GlobelPackageTypeId = input.GlobelPackageTypeId,
            GlobelPackageType = GlobalPackageTypeDTO.FromGlobalPackageType(input.GlobelPackageType),
            PackageSpecialtyId = input.PackageSpecialtyId,
            PackageSpecialty = PackageSpecialtyDto.FromPackageSpecialty( input.PackageSpecialty),
            PackageDuration = input.PackageDuration,
            ActivationDateFrom = input.ActivationDateFrom,
            ActivationDateTo = input.ActivationDateTo,
            PackagePrice = input.PackagePrice,
            PackageRoundPrice = input.PackageRoundPrice,
            IsDeleted= input.IsDeleted



        
        };
    }
}

