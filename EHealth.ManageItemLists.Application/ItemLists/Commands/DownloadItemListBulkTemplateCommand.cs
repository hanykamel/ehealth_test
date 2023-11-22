using MediatR;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands
{
    public class DownloadItemListBulkTemplateCommand : IRequest<byte[]>
    {
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEN { get; set; }
        public int? ItemListSubtypeId { get; set; }
        public int? ItemListTypeId { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
