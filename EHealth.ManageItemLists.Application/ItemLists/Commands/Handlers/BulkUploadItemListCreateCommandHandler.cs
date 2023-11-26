using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands.Handlers
{
    public class BulkUploadItemListCreateCommandHandler : IRequestHandler<BulkUploadItemListCreateCommand, byte[]?>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly IItemListTypeRepository _itemListTypeRepository;
        private readonly IItemListSubtypeRepository _itemListSubtypeRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;
        public BulkUploadItemListCreateCommandHandler(IValidationEngine validationEngine,
            IIdentityProvider identityProvider,
            IItemListTypeRepository itemListTypeRepository,
            IItemListSubtypeRepository itemListSubtypeRepository,
            IItemListRepository itemListRepository,
            IExcelService excelService)
        {
            _excelService = excelService;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _itemListTypeRepository = itemListTypeRepository;
            _itemListSubtypeRepository = itemListSubtypeRepository;
            _itemListsRepository = itemListRepository;
        }
        public async Task<byte[]?> Handle(BulkUploadItemListCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<ItemListBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 1)).ToString());

            // Throw exception if item list busy
            await ItemList.IsListBusy(_itemListsRepository, itemListId);
            byte[] bytes = null;

            try
            {

                #region lock item list
                var itemList = await ItemList.Get(itemListId, _itemListsRepository);
                itemList.SetIsBusy(true);
                itemList.ModifiedOn = DateTimeOffset.Now;
                await itemList.Update(_itemListsRepository, _validationEngine, _identityProvider.GetUserName());
                #endregion
                #region error cell style 
                XSSFCellStyle headerStyle = ExcelStyle.SetWorkbookStyle(workbook);
                var cell014 = worksheet.GetRow(0).CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 2);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 2);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var types = await ItemListType.Search(_itemListTypeRepository, f => f.IsDeleted == false, 1, 1, false);
                var subTypes = await ItemListSubtype.Search(_itemListSubtypeRepository, f => f.IsDeleted == false, 1, 1, false);
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var typeName = _excelService.GetCell(worksheet, row, (ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemType").Index))?.ToString()?.Trim();
                    var typeId = types.Data.FirstOrDefault(c => c.NameEN == typeName).Id;

                    var subTypeName = _excelService.GetCell(worksheet, row, (ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index))?.ToString()?.Trim();
                    var subTypeId = subTypes.Data.FirstOrDefault(s => s.NameEN == subTypeName).Id;


                    var updateItemListDto = new UpdateItemListDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 1)?.ToString()?.Trim())) ? 0 : int.Parse(_excelService.GetCell(worksheet, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 1)?.ToString()?.Trim()),
                        NameAr = _excelService.GetCell(worksheet, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NameAr").Index)?.ToString().Trim(),
                        NameEN = _excelService.GetCell(worksheet, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NameEN").Index)?.ToString().Trim(),
                        ItemListSubtypeId = subTypeId,
                    };



                    var itemListBulkUploadDto = new ItemListBulkUploadDto
                    {
                        UpdateItemListDto = updateItemListDto,
                        RowNumber = row
                    };
                    #endregion
                    if (updateItemListDto.Id == 0)
                    {
                        var itemLists = await _itemListsRepository.Search(f => f.ItemListSubtypeId == subTypeId, 1, 1, null, null, true);
                        var itemListSubtype = await _itemListSubtypeRepository.GetById(subTypeId);
                        string itemListNumber = $"{itemLists.TotalCount + 1}";
                        string Code = $"{itemListSubtype.Code}_{itemListNumber.PadLeft(3, '0')}";
                        var itemlist = updateItemListDto.ToItemList(Code, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await itemlist.ValidateObjectForBulkUpload(_itemListsRepository, _validationEngine);

                        itemListBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        itemListBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var itemlist = await ItemList.Get(updateItemListDto.Id, _itemListsRepository);

                        if (itemlist is null)
                        {
                            errors += "itemlist with itemlistId not exist." + "\r\n";
                        }
                        else
                        {
                            itemlist = updateItemListDto.ToItemList(itemlist.Code, _identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await itemlist.ValidateObjectForBulkUpload(_itemListsRepository, _validationEngine);


                        }
                        itemListBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        itemListBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(itemListBulkUploadDto.errors.Contains("Duplicated Code")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Code").Index, "Duplicated Code");
                    }

                    if (!(itemListBulkUploadDto.errors.Contains("Duplicated NameAr")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NameAr").Index, "Duplicated NameAr");
                    }

                    if (!(itemListBulkUploadDto.errors.Contains("Duplicated NameEN")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NameEN").Index, "Duplicated NameEN");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        itemListBulkUploadDto.IsValid = false;
                        itemListBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!itemListBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 2);
                        cellRow14.SetCellValue(itemListBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 2);
                        cellRow14.SetCellValue("");
                    }
                    list.Add(itemListBulkUploadDto);

                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.UpdateItemListDto.Id == 0)
                        {
                            var itemLists = await _itemListsRepository.Search(f => f.ItemListSubtypeId == item.UpdateItemListDto.ItemListSubtypeId, 1, 1, null, null, true);
                            var itemListSubtype = await _itemListSubtypeRepository.GetById(item.UpdateItemListDto.ItemListSubtypeId);
                            string itemListNumber = $"{itemLists.TotalCount + 1}";
                            string Code = $"{itemListSubtype.Code}_{itemListNumber.PadLeft(3, '0')}";
                            var ItemLisrWithoutId = item.UpdateItemListDto.ToItemList(Code, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await ItemLisrWithoutId.Create(_itemListsRepository, _validationEngine);

                            var itemListAdded = await ItemList.Get(ItemLisrWithoutId.Id, _itemListsRepository);

                            #region update Cells of added row (id)

                            var CellId1 = _excelService.GetCell(worksheet, item.RowNumber, ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Code").Index);
                            CellId1.SetCellValue(itemListAdded?.Code.ToString());
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 1);
                            CellId.SetCellValue(itemListAdded?.Id.ToString());

                            #endregion
                        }
                        else
                        {
                            var itemListUpdated = await ItemList.Get(item.UpdateItemListDto.Id, _itemListsRepository);
                            itemListUpdated.SetNameAr(item.UpdateItemListDto.NameAr);
                            itemListUpdated.SetNameEn(item.UpdateItemListDto.NameEN);

                            itemListUpdated.SetModifiedBy(_identityProvider.GetUserName());
                            itemListUpdated.SetModifiedOn();



                            await itemListUpdated.Update(_itemListsRepository, _validationEngine, userId);
                        }
                    }
                }
                if (list.Any(x => x.IsValid == false))
                {


                    MemoryStream output = new MemoryStream();
                    workbook.Write(output);
                    bytes = output.ToArray();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                #region unlock item list
                var itemList = await ItemList.Get(itemListId, _itemListsRepository);
                itemList.SetIsBusy(false);
                itemList.ModifiedOn = DateTimeOffset.Now;
                await itemList.Update(_itemListsRepository, _validationEngine, _identityProvider.GetUserName());
                #endregion

            }
            return bytes;
        }
    }
}
