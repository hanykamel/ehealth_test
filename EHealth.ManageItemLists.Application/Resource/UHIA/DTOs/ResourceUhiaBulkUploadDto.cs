namespace EHealth.ManageItemLists.Application.Resource.UHIA.DTOs
{
    public class ResourceUhiaBulkUploadDto
    {
        public UpdateResourceUHIABasicDataDto updateResourceUHIABasicDataDto { get; set; }
        public UpdateResourceUHIAPriceDto updateResourceUHIAPriceDto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
