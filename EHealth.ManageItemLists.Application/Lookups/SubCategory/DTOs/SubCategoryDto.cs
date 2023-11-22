using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string SubCategoryAr { get; set; }
        public string SubCategoryEn { get; set; }
        public string? DefinitionAr { get; set; }
        public string? DefinitionEn { get; set; }
        public CategoryDto Category { get; set; }
        public int  ItemListSubtypeId { get; set; }
        public bool IsDeleted { get; set; }


        public static SubCategoryDto FromSubCategory(SubCategory input) =>
       input is not null? new SubCategoryDto
        {
            Id = input.Id,
            SubCategoryAr = input.SubCategoryAr,
            SubCategoryEn = input.SubCategoryEn,
            DefinitionAr = input.DefinitionAr,
            DefinitionEn = input.DefinitionEn,
            Category = CategoryDto.FromCategory(input.Category),
            ItemListSubtypeId = input.ItemListSubtypeId,
            IsDeleted = input.IsDeleted
        } : null;
    }
}
