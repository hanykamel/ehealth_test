namespace EHealth.ManageItemLists.Application.ItemLists.DTOs
{
    public class ItemListBulkUploadDto
    {
        public UpdateItemListDto UpdateItemListDto  { get; set; }
     

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
