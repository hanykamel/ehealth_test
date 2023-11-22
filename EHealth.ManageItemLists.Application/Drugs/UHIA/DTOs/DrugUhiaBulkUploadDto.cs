

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class DrugUhiaBulkUploadDto
    {
        public UpdateDrugUHIABasicDataDto updateDrugUHIABasicDataDto { get; set; }
        public UpdateDrugUHIAPriceDto updateDrugUHIAPriceDto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
