using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs
{
    public class CreateDoctorFeesUHIAPriceDto
    {
        public Guid DoctorFeesUHIAId { get; set; }
        public List<CreateDoctorFeesItemPriceDto> ItemListPrices { get; set; } = new List<CreateDoctorFeesItemPriceDto>();
    }
}
