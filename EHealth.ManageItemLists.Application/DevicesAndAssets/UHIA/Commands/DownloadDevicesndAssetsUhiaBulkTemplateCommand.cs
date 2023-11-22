using MediatR;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands
{
    public class DownloadDevicesndAssetsUhiaBulkTemplateCommand : IRequest<byte[]>
    {
       
        public int ItemListId { get; set; }
        public int ItemListSubtypeId { get; set; }
        public string? EHealthCode { get; set; }
        public string? DescriptorEn { get; set; }
        public string? DescriptorAr { get; set; }
        public string? UnitRoomEn { get; set; }
        public string? CategoryEn { get; set; }
        public string? SubCategoryEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
