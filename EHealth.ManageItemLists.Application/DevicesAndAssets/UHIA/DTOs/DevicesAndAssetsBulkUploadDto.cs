using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs
{
    public class DevicesAndAssetsBulkUploadDto
    {
        public UpdateDevicesAndAssetsUHIABasicDataDto  updateDevicesAndAssetsUHIABasicDataDto { get; set; }
        public UpdateDevicesAndAssetsUHIAPriceDto updateDevicesAndAssetsUHIAPriceDto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
