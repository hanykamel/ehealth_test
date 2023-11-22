using MediatR;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands
{
    public class DownloadDoctorFeesUhiaBulkTemplateCommand : IRequest<byte[]>
    {
        public int ItemListId { get; set; }
        public int itemListSubtypeId { get; set; }
        public string? EHealthCode { get; set; }
        public string? DescriptorEn { get; set; }
        public string? DescriptorAr { get; set; }
        public string? ComplexityClassificationCode { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
