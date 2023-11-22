using EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice;

namespace EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs
{
    public class UpdateDoctorFeesItemPriceDto
    {
        public int Id { get; set; }
        public double DoctorFees { get; set; }
        public int UnitOfDoctorFeesId { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }
        public DoctorFeesItemPrice ToDrFeesItemPrice(string createdBy, string tenantId) => DoctorFeesItemPrice.Create(null, DoctorFees, UnitOfDoctorFeesId, EffectiveDateFrom, EffectiveDateTo, createdBy, tenantId);
    }
}
