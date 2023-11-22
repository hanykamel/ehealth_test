
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.UnitRooms;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Handlers
{
    public class BulkUploadDevicesAndAssetsCreateCommandHandler : IRequestHandler<BulkUploadDevicesAndAssetsCreateCommand, byte[]?>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly IUnitRoomRepository _unitRoomRepository ;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;
        public BulkUploadDevicesAndAssetsCreateCommandHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository,
             IValidationEngine validationEngine,
        IIdentityProvider identityProvider,
        ICategoriesRepository categoriesRepository,
        ISubCategoriesRepository subCategoriesRepository,
        IUnitRoomRepository unitRoomRepository,
        IItemListRepository itemListsRepository,
        IExcelService excelService)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _categoriesRepository = categoriesRepository;
            _subCategoriesRepository = subCategoriesRepository;
            _itemListsRepository = itemListsRepository;
            _excelService = excelService;
            _unitRoomRepository = unitRoomRepository;
        }

        public async Task<byte[]?> Handle(BulkUploadDevicesAndAssetsCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<DevicesAndAssetsBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await DevicesAndAssetsUHIA.IsItemListBusy(_devicesAndAssetsUHIARepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var categories = await Domain.Categories.Category.Search(_categoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var subcategories = await Domain.Sub_Categories.SubCategory.Search(_subCategoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var unitRooms = await UnitRoom.Search(_unitRoomRepository, f => f.IsDeleted == false, 1, 1, false);
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var catName = _excelService.GetCell(worksheet, row, (DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index))?.ToString()?.Trim();
                    var catId = categories.Data.FirstOrDefault(c => c.CategoryEn == catName).Id;

                    var subCatName = _excelService.GetCell(worksheet, row, (DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index))?.ToString()?.Trim();
                    var subCatId = subcategories.Data.FirstOrDefault(s => s.SubCategoryEn == subCatName).Id;

                    var unitRoomName = _excelService.GetCell(worksheet, row, (DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UnitRoom").Index))?.ToString()?.Trim();
                    var unitRoomId = unitRooms.Data.FirstOrDefault(s => s.NameEN == unitRoomName).Id;


                    var itemListPrices = new List<UpdateItemListPriceDto>();
                    UpdateItemListPriceDto? updateItemListPriceDto = null;


                    if (_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim())) &&
                         _excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index))?.ToString().Trim())))
                    {
                        updateItemListPriceDto = new UpdateItemListPriceDto()
                        {
                            Id = (_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3) == null ||
                            string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim())) ? 0
                            : int.Parse(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim()),

                            Price = double.Parse((_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim()),
                            EffectiveDateFrom = Convert.ToDateTime((_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index)).ToString().Trim()).Date,
                            EffectiveDateTo = (_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index) == null
                            || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim())) ? null
                            : Convert.ToDateTime(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        };
                    
                        itemListPrices.Add(updateItemListPriceDto);
                    }


                    var updateDevicesAndAssetsUHIABasicDataDto = new UpdateDevicesAndAssetsUHIABasicDataDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index)?.ToString().Trim(),
                        DescriptorAr = _excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index)?.ToString().Trim(),
                        DescriptorEn = _excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index)?.ToString().Trim(),
                        CategoryId = catId,
                        SubCategoryId = subCatId,
                        UnitRoomId=unitRoomId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };


                    var devicesAndAssetsBulkUploadDto = new DevicesAndAssetsBulkUploadDto
                    {
                        updateDevicesAndAssetsUHIABasicDataDto = updateDevicesAndAssetsUHIABasicDataDto,
                        updateDevicesAndAssetsUHIAPriceDto = new UpdateDevicesAndAssetsUHIAPriceDto 
                        {
                            ItemListPrices = itemListPrices,
                            DevicesAndAssetsUHIAId = (_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim()),
                        },
                        RowNumber = row
                    };
                    #endregion
                    if (updateDevicesAndAssetsUHIABasicDataDto.Id == Guid.Empty)
                    {
                        var devAndAssetsUhia = updateDevicesAndAssetsUHIABasicDataDto.ToDevAndAssetsUHIA(userId, tenantId);
                        errors = await devAndAssetsUhia.ValidateObjectForBulkUpload(_devicesAndAssetsUHIARepository, _validationEngine);
                        foreach (var item in devicesAndAssetsBulkUploadDto.updateDevicesAndAssetsUHIAPriceDto.ItemListPrices)
                        {
                            var itemListPrice = item.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            var itemErrors = _validationEngine.Validate(itemListPrice, false);
                            if (itemErrors != null)
                            {
                                foreach (var error in itemErrors)
                                {
                                    errors += error.ErrorMessage + "\r\n";
                                }
                            }

                            if (item.EffectiveDateFrom.Date < devAndAssetsUhia.DataEffectiveDateFrom.Date ||
                                (item.EffectiveDateTo.HasValue && devAndAssetsUhia.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > devAndAssetsUhia.DataEffectiveDateTo.Value.Date) ||
                                ((!item.EffectiveDateTo.HasValue) && devAndAssetsUhia.DataEffectiveDateTo.HasValue))
                            {
                                errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                            }
                        }

                        devicesAndAssetsBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        devicesAndAssetsBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var devAndAssetsUhia = await DevicesAndAssetsUHIA.Get(updateDevicesAndAssetsUHIABasicDataDto.Id, _devicesAndAssetsUHIARepository);

                        if (devAndAssetsUhia is null)
                        {
                            errors += "ServiceUHIA with ServiceUHIAId not exist." + "\r\n";
                        }
                        else
                        {
                            devAndAssetsUhia = updateDevicesAndAssetsUHIABasicDataDto.ToDevAndAssetsUHIA(userId, tenantId);

                            errors = await devAndAssetsUhia.ValidateObjectForBulkUpload(_devicesAndAssetsUHIARepository, _validationEngine);

                            var notDeletedItems = devAndAssetsUhia.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                            foreach (var item in notDeletedItems)
                            {
                                if (item.EffectiveDateFrom.Date < updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                    break;
                                }
                            }

                            foreach (var item in devicesAndAssetsBulkUploadDto.updateDevicesAndAssetsUHIAPriceDto.ItemListPrices)
                            {
                                var itemListPrice = item.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                var itemErrors = _validationEngine.Validate(itemListPrice, false);
                                if (itemErrors != null)
                                {
                                    foreach (var error in itemErrors)
                                    {
                                        errors += error.ErrorMessage + "\r\n";
                                    }
                                }

                                if (item.EffectiveDateFrom.Date < updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                }
                                if (item.Id != 0)
                                {
                                    notDeletedItems = notDeletedItems.Where(x => x.Id != item.Id).ToList();
                                }
                                notDeletedItems.Add(item.ToItemListPrice(_identityProvider.GetUserId(), _identityProvider.GetTenantId()));
                            }


                            var convertedItemLst = new List<DateRangeDto>();
                            foreach (var item in notDeletedItems)
                            {
                                var convertedItem = new DateRangeDto
                                {
                                    Start = item.EffectiveDateFrom.Date,
                                    End = item.EffectiveDateTo.HasValue ? item.EffectiveDateTo.Value.Date : null
                                };
                                convertedItemLst.Add(convertedItem);
                            }

                            if (!DateAndTimeOperations.DoesNotOverlap(convertedItemLst))
                            {
                                errors += "The dates overlap with those already specified. Please enter additional dates." + "\r\n";
                            }

                        }
                        devicesAndAssetsBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        devicesAndAssetsBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(devicesAndAssetsBulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index, "Duplicated EHealthCode");
                    }

                    if (!(devicesAndAssetsBulkUploadDto.errors.Contains("Duplicated ShortDescAr")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index, "Duplicated ShortDescAr");
                    }

                    if (!(devicesAndAssetsBulkUploadDto.errors.Contains("Duplicated ShortDescEn")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index, "Duplicated ShortDescEn");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        devicesAndAssetsBulkUploadDto.IsValid = false;
                        devicesAndAssetsBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!devicesAndAssetsBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue(devicesAndAssetsBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue("");
                    }
                    list.Add(devicesAndAssetsBulkUploadDto);
                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.updateDevicesAndAssetsUHIABasicDataDto.Id == Guid.Empty)
                        {
                            var devAndAssetsWithoutId = item.updateDevicesAndAssetsUHIABasicDataDto.ToDevAndAssetsUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await devAndAssetsWithoutId.Create(_devicesAndAssetsUHIARepository, _validationEngine);

                            var devAndAssetsUhia = await DevicesAndAssetsUHIA.Get(devAndAssetsWithoutId.Id, _devicesAndAssetsUHIARepository);

                            foreach (var price in item.updateDevicesAndAssetsUHIAPriceDto.ItemListPrices)
                            {
                                var itemListPrice = price.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                devAndAssetsUhia.ItemListPrices.Add(itemListPrice);
                            }
                            await devAndAssetsUhia.Update(_devicesAndAssetsUHIARepository, _validationEngine, _identityProvider.GetUserName());
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                            CellId.SetCellValue(devAndAssetsUhia?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);
                            var cellpriceId = worksheet.GetRow(item.RowNumber).CreateCell(DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3);
                            cellpriceId.SetCellValue(devAndAssetsUhia?.ItemListPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()?.ToString());
                            #endregion
                        }
                        else
                        {
                            var devAndAssetsUhia = await DevicesAndAssetsUHIA.Get(item.updateDevicesAndAssetsUHIABasicDataDto.Id, _devicesAndAssetsUHIARepository);
                            devAndAssetsUhia.SetEHealthCode(item.updateDevicesAndAssetsUHIABasicDataDto.EHealthCode);
                            devAndAssetsUhia.SetDescriptorAr(item.updateDevicesAndAssetsUHIABasicDataDto.DescriptorAr);
                            devAndAssetsUhia.SetDescriptorEn(item.updateDevicesAndAssetsUHIABasicDataDto.DescriptorEn);
                            devAndAssetsUhia.SetUnitRoomId(item.updateDevicesAndAssetsUHIABasicDataDto.UnitRoomId);
                            devAndAssetsUhia.SetCategoryId(item.updateDevicesAndAssetsUHIABasicDataDto.CategoryId);
                            devAndAssetsUhia.SetSubCategoryId(item.updateDevicesAndAssetsUHIABasicDataDto.SubCategoryId);
                            devAndAssetsUhia.SetDataEffectiveDateFrom(item.updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateFrom);
                            devAndAssetsUhia.SetDataEffectiveDateTo(item.updateDevicesAndAssetsUHIABasicDataDto.DataEffectiveDateTo);
                            devAndAssetsUhia.SetModifiedBy(_identityProvider.GetUserName());
                            devAndAssetsUhia.SetModifiedOn();

                            // prepare model to update and soft delete Item Prices
                            for (int i = 0; i < devAndAssetsUhia.ItemListPrices.Count; i++)
                            {
                                var itemPrice = item.updateDevicesAndAssetsUHIAPriceDto.ItemListPrices.Where(x => x.Id == devAndAssetsUhia.ItemListPrices[i].Id).FirstOrDefault();
                                if (itemPrice == null)
                                {
                                    continue; // do not apply soft delete now

                                    //serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                                    //serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                                }
                                else
                                {
                                    devAndAssetsUhia.ItemListPrices[i].SetPrice(itemPrice.Price);
                                    devAndAssetsUhia.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                                    devAndAssetsUhia.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                                    devAndAssetsUhia.ItemListPrices[i].SetModifiedBy(userId);
                                }
                                devAndAssetsUhia.ItemListPrices[i].SetModifiedOn();
                            }

                            // prepare model to add new Item Prices
                            var addItemPrices = item.updateDevicesAndAssetsUHIAPriceDto.ItemListPrices.Where(x => x.Id == 0).ToList();
                            foreach (var addedItem in addItemPrices)
                            {
                                var itemListPrice = addedItem.ToItemListPrice(userId, tenantId);
                                devAndAssetsUhia.ItemListPrices.Add(itemListPrice);
                            }

                            await devAndAssetsUhia.Update(_devicesAndAssetsUHIARepository, _validationEngine,_identityProvider.GetUserName());
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
