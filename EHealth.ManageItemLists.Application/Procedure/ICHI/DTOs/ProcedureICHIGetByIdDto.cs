using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs
{
    public class ProcedureICHIGetByIdDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public CategoryDto Category { get; set; }
        public LocalSpecialtyDepartmentDto LocalSpecialtyDepartment { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public int? LocalSpecialtyDepartmentId { get; set; }
        public IList<ItemListPriceDto> ItemListPrices { get; set; } = new List<ItemListPriceDto>();
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public bool IsDeleted { get; set; }

        public static ProcedureICHIGetByIdDto FromProcedureIVHIGetById(ProcedureICHI input) =>
     new ProcedureICHIGetByIdDto
     {
         Id = input.Id,
         EHealthCode = input.EHealthCode,
         TitleAr = input.TitleAr,
         TitleEn = input.TitleEn,
         UHIAId = input.UHIAId,
         LocalSpecialtyDepartment= LocalSpecialtyDepartmentDto.FromLLocalSpecialityDepartment(input.LocalSpecialtyDepartment),
         Category = CategoryDto.FromCategory(input.ServiceCategory),
         SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
         DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
         DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
         ItemListPrices = ItemListPriceDto.FromItemPrice(input.ItemListPrices),
         ServiceSubCategoryId = input.SubCategoryId,
         ItemListId = input.ItemListId,
         LocalSpecialtyDepartmentId = input.LocalSpecialtyDepartmentId,
         ServiceCategoryId = input.ServiceCategoryId,
         IsDeleted = input.IsDeleted,
     };
    }
}
