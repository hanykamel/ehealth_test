using EHealth.ManageItemLists.Application.Lookups.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitRooms.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs
{
    public class DevicesAndAssetsUHIAGetByIdDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public UnitRoomDto? UnitRoom { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public CategoryDto Category { get; set; }
        public int? UnitRoomId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public IList<ItemListPriceDto> ItemListPrices { get; set; } = new List<ItemListPriceDto>();
        public string? ModifiedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public static DevicesAndAssetsUHIAGetByIdDto FromDevicesAndAssetsUHIAGetById(DevicesAndAssetsUHIA input) =>
new DevicesAndAssetsUHIAGetByIdDto
{
    Id = input.Id,
    EHealthCode = input.Code,
    DescriptorAr = input.DescriptorAr,
    DescriptorEn = input.DescriptorEn,
    UnitRoom = UnitRoomDto.FromUnitRoom(input.UnitRoom),
    Category = CategoryDto.FromCategory(input.Category),
    SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
    DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
    DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
    ItemListPrices = ItemListPriceDto.FromItemPrice(input.ItemListPrices),
    ModifiedBy = input.ModifiedBy,
    ModifiedOn = input.ModifiedOn?.ToString("yyyy-MM-dd"),
    ItemListId = input.ItemListId,
    CategoryId = input.CategoryId,
    SubCategoryId = input.SubCategoryId,
    UnitRoomId = input.UnitRoomId,
    IsDeleted = input.IsDeleted
};
    }
}
