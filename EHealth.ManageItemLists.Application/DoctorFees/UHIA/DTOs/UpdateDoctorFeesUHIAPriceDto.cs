using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs
{
    public class UpdateDoctorFeesUHIAPriceDto
    {
        public Guid DoctorFeesUHIAId { get; set; }
        public List<UpdateDoctorFeesItemPriceDto> ItemListPrices { get; set; } = new List<UpdateDoctorFeesItemPriceDto>();
    }
}
