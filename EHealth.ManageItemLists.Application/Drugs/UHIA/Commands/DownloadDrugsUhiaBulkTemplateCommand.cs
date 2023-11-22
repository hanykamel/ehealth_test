using MediatR;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands
{
    public class DownloadDrugsUhiaBulkTemplateCommand : IRequest<byte[]>
    {
        public int ItemListId { get; set; }
        public int ItemListSubtypeId { get; set; }
        public string? EHealthCode { get; set; }
        public string? LocalDrugCode { get; set; }
        public string? InternationalNonProprietaryName { get; set; }
        public string? ProprietaryName { get; set; }
        public string? DosageForm { get; set; }
        public string? RouteOfAdministration { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
