using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.DTOs
{
    public class UpdateResourceUHIAPriceDto
    {
        public Guid ResourceUHIAId { get; set; }
        public List<UpdateResourceItemPriceDto> ResourceItemPrices { get; set; } = new List<UpdateResourceItemPriceDto>();
    }
}
