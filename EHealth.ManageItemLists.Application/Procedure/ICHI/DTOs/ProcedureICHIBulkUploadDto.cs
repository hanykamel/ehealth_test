namespace EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs
{
    public class ProcedureICHIBulkUploadDto
    {
        public UpdateProcedureICHIBasicDataDto updateProcedureICHIBasicDataDto { get; set; }
        public UpdateProcedureICHIPriceDto  updateProcedureICHIPriceDto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
