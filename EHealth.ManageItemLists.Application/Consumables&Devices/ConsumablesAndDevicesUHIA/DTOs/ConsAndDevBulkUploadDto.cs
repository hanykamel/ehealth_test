namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs
{
    public class ConsAndDevBulkUploadDto
    {
        public UpdateConsAndDevUHIABasicDataDto updateConsAndDevUHIABasicDataDto { get; set; }
        public UpdateConsAndDevUHIAPriceDto updateConsAndDevUHIAPriceDto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
