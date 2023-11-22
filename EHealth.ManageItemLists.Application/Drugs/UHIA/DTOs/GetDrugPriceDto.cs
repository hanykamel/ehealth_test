using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice;
using EHealth.ManageItemLists.Domain.DrugsPricing;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class GetDrugPriceDto
    {
        public int Id { get; private set; }
        public double MainUnitPrice { get; private set; }
        public double FullPackPrice { get; private set; }
        public double SubUnitPrice { get; private set; }
        public string EffectiveDateFrom { get; private set; }
        public string? EffectiveDateTo { get; private set; }
        public bool IsDeleted { get; set; }

        public static GetDrugPriceDto FromDrugPriceDto(DrugPrice input) =>
        input is not null ? new GetDrugPriceDto
        {
            Id = input.Id,
            MainUnitPrice = input.MainUnitPrice,
            FullPackPrice = input.FullPackPrice,
            SubUnitPrice = input.SubUnitPrice,
            EffectiveDateFrom = input.EffectiveDateFrom.ToString("yyyy-MM-dd"),
            EffectiveDateTo = input.EffectiveDateTo?.ToString("yyyy-MM-dd"),
            IsDeleted = input.IsDeleted,
        } : null;
    }
}
