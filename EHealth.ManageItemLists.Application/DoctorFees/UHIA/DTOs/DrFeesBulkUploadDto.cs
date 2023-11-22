namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs
{
    public class DrFeesBulkUploadDto
    {
        public UpdateDoctoerFeesUHIABasicDataDto  updateDoctoerFeesUHIABasicDataDto { get; set; }
        public UpdateDoctorFeesUHIAPriceDto updateDoctorFeesUHIAPriceDto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
