using MediatR;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands
{
    public class DownloadProcedureICHIBulkTemplateCommand : IRequest<byte[]>
    {
        public int ItemListId { get; set; }
        public int ItemListSubtypeId { get; set; }
        public string? EHealthCode { get; set; }
        public string? UHIAId { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
