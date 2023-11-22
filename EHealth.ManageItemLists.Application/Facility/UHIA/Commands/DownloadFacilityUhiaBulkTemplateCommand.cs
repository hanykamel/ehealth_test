using MediatR;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands
{
    public class DownloadFacilityUhiaBulkTemplateCommand : IRequest<byte[]>
    {
        public int ItemListId { get; set; }
        public int ItemListSubtypeId { get; set; }
        public string? Code { get; set; }
        public string? DescriptorAr { get; set; }
        public string? DescriptorEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
