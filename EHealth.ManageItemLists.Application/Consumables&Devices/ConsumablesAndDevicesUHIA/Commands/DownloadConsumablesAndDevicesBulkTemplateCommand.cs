using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands
{
    public class DownloadConsumablesAndDevicesBulkTemplateCommand : IRequest<byte[]>
    {
        public int ItemListId { get; set; }
        public int ItemListSubtypeId { get; set; }
        public string? EHealthCode { get; set; }
        public string? UHIAId { get; set; }
        public string? ShortDescriptionAr { get; set; }
        public string? ShortDescriptionEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
