using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs
{
    public class ProcedureICHIDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public CategoryDto Category { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public int? LocalSpecialtyDepartmentId { get; private set; }
        public LocalSpecialtyDepartmentDto? LocalSpecialtyDepartment { get; private set; }

        public string DataEffectiveDateFrom { get; private set; }
        public string? DataEffectiveDateTo { get; private set; }
        public ItemListPriceDto ItemListPrice { get; set; }
        public int ItemListId { get; set; }
        public bool IsDeleted { get; set; }

        public static ProcedureICHIDto FromProcedureICHI(ProcedureICHI input) =>
       new ProcedureICHIDto
       {
           Id = input.Id,
           EHealthCode = input.EHealthCode,
           ItemListId = input.ItemListId,
           UHIAId = input.UHIAId,
           TitleAr = input.TitleAr,
           TitleEn = input.TitleEn,
           Category = CategoryDto.FromCategory(input.ServiceCategory),
           SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
           LocalSpecialtyDepartmentId = input.LocalSpecialtyDepartmentId,
           LocalSpecialtyDepartment = LocalSpecialtyDepartmentDto.FromLLocalSpecialityDepartment(input.LocalSpecialtyDepartment),
           DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
           DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
           ItemListPrice = ItemListPriceDto.FromItemListPrice(input.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()),
           IsDeleted=input.IsDeleted,
       };
    }
}
