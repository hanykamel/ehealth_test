using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.Categories.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }
        public string DefinitionAr { get; set; }
        public string DefinitionEn { get; set; }
        public int ItemListSubtypeId { get; set; }
        public bool IsDeleted { get; set; }


        public static CategoryDto FromCategory(Category input) =>
         input is not null ? new CategoryDto
        {
            Id = input.Id,
            CategoryAr = input.CategoryAr,
            CategoryEn = input.CategoryEn,
            DefinitionAr = input.DefinitionAr,
            DefinitionEn = input.DefinitionEn,
            ItemListSubtypeId = input.ItemListSubtypeId,
            IsDeleted = input.IsDeleted,
        }:null;
    }
}
