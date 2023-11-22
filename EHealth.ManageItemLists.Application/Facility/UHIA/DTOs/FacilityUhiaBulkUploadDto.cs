using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.DTOs
{
    public class FacilityUhiaBulkUploadDto
    {
        public UpdateFacilityUHIADto updateFacilityUHIADto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
