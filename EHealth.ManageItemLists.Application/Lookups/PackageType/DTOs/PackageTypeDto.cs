using EHealth.ManageItemLists.Domain.DrugsPackageTypes;

namespace EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs
{
    public class PackageTypeDto
    {
        public int Id { get; set; }
        public string NameAr { get;  set; }
        public string NameEN { get;  set; }
        public string? DefinitionAr { get;  set; }
        public string? DefinitionEN { get;  set; }
        public bool IsDeleted { get; set; }

        public static PackageTypeDto FromPackageType(EHealth.ManageItemLists.Domain.PackageTypes.PackageType input) =>
        input is not null ? new PackageTypeDto
        {
            Id = input.Id,
            NameAr = input.NameAr,
            NameEN = input.NameEN,
            //PackageSubTypes = FromPackageTypes(input.PackageSubTypes)
        } : null;

    }
}
