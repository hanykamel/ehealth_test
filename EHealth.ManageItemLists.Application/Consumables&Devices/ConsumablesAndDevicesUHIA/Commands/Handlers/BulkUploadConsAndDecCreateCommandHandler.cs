using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Handlers
{
    public class BulkUploadConsAndDecCreateCommandHandler : IRequestHandler<BulkUploadConsAndDevsCreateCommand, byte[]?>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;
        public BulkUploadConsAndDecCreateCommandHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider,
        ICategoriesRepository categoriesRepository,
        ISubCategoriesRepository subCategoriesRepository,
        IUnitOfMeasureRepository unitOfMeasureRepository,
        IItemListRepository itemListsRepository,
        IExcelService excelService)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _categoriesRepository = categoriesRepository;
            _subCategoriesRepository = subCategoriesRepository;
            _itemListsRepository = itemListsRepository;
            _excelService = excelService;
            _unitOfMeasureRepository = unitOfMeasureRepository;
        }

        public async Task<byte[]?> Handle(BulkUploadConsAndDevsCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<ConsAndDevBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.IsItemListBusy(_consumablesAndDevicesUHIARepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var categories = await Domain.Categories.Category.Search(_categoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var subcategories = await Domain.Sub_Categories.SubCategory.Search(_subCategoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var lcalUnitOfMeasures = await UnitOfMeasure.Search(_unitOfMeasureRepository, f => f.IsDeleted == false, 1, 1, false);
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var catName = _excelService.GetCell(worksheet, row, (ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index))?.ToString()?.Trim();
                    var catId = categories.Data.FirstOrDefault(c => c.CategoryEn == catName).Id;

                    var subCatName = _excelService.GetCell(worksheet, row, (ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index))?.ToString()?.Trim();
                    var subCatId = subcategories.Data.FirstOrDefault(s => s.SubCategoryEn == subCatName).Id;

                    var unitOfMeasureName = _excelService.GetCell(worksheet, row, (ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "LocalUnitOfMeasure").Index))?.ToString()?.Trim();
                    var unitOfMeasureId = lcalUnitOfMeasures.Data.FirstOrDefault(s => s.MeasureTypeENG == unitOfMeasureName).Id;


                    var itemListPrices = new List<UpdateItemListPriceDto>();
                    UpdateItemListPriceDto? updateItemListPriceDto = null;


                    if (_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim())) &&
                         _excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index))?.ToString().Trim())))
                    {
                        updateItemListPriceDto = new UpdateItemListPriceDto()
                        {
                            Id = (_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3) == null ||
                            string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim())) ? 0
                            : int.Parse(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim()),

                            Price = double.Parse((_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim()),
                            EffectiveDateFrom = Convert.ToDateTime((_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index)).ToString().Trim()).Date,
                            EffectiveDateTo = (_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index) == null
                            || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim())) ? null
                            : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        };
                        itemListPrices.Add(updateItemListPriceDto);
                    }


                    var updateConsAndDevUHIABasicDataDto = new UpdateConsAndDevUHIABasicDataDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index)?.ToString().Trim(),
                        UHIAId = _excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UHIAId").Index)?.ToString().Trim(),
                        ShortDescAr = _excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescriptionAr").Index)?.ToString().Trim(),
                        ShortDescEn = _excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescriptionEn").Index)?.ToString().Trim(),
                        ServiceCategoryId = catId,
                        ServiceSubCategoryId = subCatId,
                        LocalUnitOfMeasureId = unitOfMeasureId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };


                    var consAndDevBulkUploadDto = new ConsAndDevBulkUploadDto
                    {
                        updateConsAndDevUHIABasicDataDto = updateConsAndDevUHIABasicDataDto,
                        updateConsAndDevUHIAPriceDto = new UpdateConsAndDevUHIAPriceDto
                        {
                            ItemListPrices = itemListPrices,
                            ConsAndDevUHIAId = (_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim()),
                        },
                        RowNumber = row
                    };
                    #endregion
                    if (updateConsAndDevUHIABasicDataDto.Id == Guid.Empty)
                    {
                        var consAndDevUhia = updateConsAndDevUHIABasicDataDto.ToConsAndDevUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await consAndDevUhia.ValidateObjectForBulkUpload(_consumablesAndDevicesUHIARepository, _validationEngine);
                        foreach (var item in consAndDevBulkUploadDto.updateConsAndDevUHIAPriceDto.ItemListPrices)
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

                            if (item.EffectiveDateFrom.Date < consAndDevUhia.DataEffectiveDateFrom.Date ||
                                (item.EffectiveDateTo.HasValue && consAndDevUhia.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > consAndDevUhia.DataEffectiveDateTo.Value.Date) ||
                                ((!item.EffectiveDateTo.HasValue) && consAndDevUhia.DataEffectiveDateTo.HasValue))
                            {
                                errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                            }
                        }

                        consAndDevBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        consAndDevBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var consAndDevUhia = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(updateConsAndDevUHIABasicDataDto.Id, _consumablesAndDevicesUHIARepository);

                        if (consAndDevUhia is null)
                        {
                            errors += "ConsumablesAndDevicesUHIA with ConsumablesAndDevicesUHIAId not exist." + "\r\n";
                        }
                        else
                        {
                            consAndDevUhia = updateConsAndDevUHIABasicDataDto.ToConsAndDevUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await consAndDevUhia.ValidateObjectForBulkUpload(_consumablesAndDevicesUHIARepository, _validationEngine);

                            var notDeletedItems = consAndDevUhia.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                            foreach (var item in notDeletedItems)
                            {
                                if (item.EffectiveDateFrom.Date < updateConsAndDevUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateConsAndDevUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateConsAndDevUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateConsAndDevUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                    break;
                                }
                            }

                            foreach (var item in consAndDevBulkUploadDto.updateConsAndDevUHIAPriceDto.ItemListPrices)
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

                                if (item.EffectiveDateFrom.Date < updateConsAndDevUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateConsAndDevUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateConsAndDevUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateConsAndDevUHIABasicDataDto.DataEffectiveDateTo.HasValue))
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
                        consAndDevBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        consAndDevBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(consAndDevBulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index, "Duplicated EHealthCode");
                    }

                    if (!(consAndDevBulkUploadDto.errors.Contains("Duplicated UHIAId")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UHIAId").Index, "Duplicated UHIAId");
                    }

                    if (!(consAndDevBulkUploadDto.errors.Contains("Duplicated ShortDescAr")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescriptionAr").Index, "Duplicated ShortDescAr");
                    }

                    if (!(consAndDevBulkUploadDto.errors.Contains("Duplicated ShortDescEn")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescriptionEn").Index, "Duplicated ShortDescEn");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        consAndDevBulkUploadDto.IsValid = false;
                        consAndDevBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!consAndDevBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue(consAndDevBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue("");


                    }


                    list.Add(consAndDevBulkUploadDto);

                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.updateConsAndDevUHIABasicDataDto.Id == Guid.Empty)
                        {
                            var consAndDevWithoutId = item.updateConsAndDevUHIABasicDataDto.ToConsAndDevUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await consAndDevWithoutId.Create(_consumablesAndDevicesUHIARepository, _validationEngine);

                            var consAndDevUhia = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(consAndDevWithoutId.Id, _consumablesAndDevicesUHIARepository);

                            foreach (var price in item.updateConsAndDevUHIAPriceDto.ItemListPrices)
                            {
                                var itemListPrice = price.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                consAndDevUhia.ItemListPrices.Add(itemListPrice);
                            }
                            await consAndDevUhia.Update(_consumablesAndDevicesUHIARepository, _validationEngine);
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                            CellId.SetCellValue(consAndDevUhia?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);
                            var cellpriceId = worksheet.GetRow(item.RowNumber).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3);
                            cellpriceId.SetCellValue(consAndDevUhia?.ItemListPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()?.ToString());
                            #endregion
                        }
                        else
                        {
                            var consAndDevUhia = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(item.updateConsAndDevUHIABasicDataDto.Id, _consumablesAndDevicesUHIARepository);
                            consAndDevUhia.SetEHealthCode(item.updateConsAndDevUHIABasicDataDto.EHealthCode);
                            consAndDevUhia.SetUHIAId(item.updateConsAndDevUHIABasicDataDto.UHIAId);
                            consAndDevUhia.SetShortDescAr(item.updateConsAndDevUHIABasicDataDto.ShortDescAr);
                            consAndDevUhia.SetShortDescEn(item.updateConsAndDevUHIABasicDataDto.ShortDescEn);
                            consAndDevUhia.SetUnitOfMeasureId(item.updateConsAndDevUHIABasicDataDto.LocalUnitOfMeasureId);
                            consAndDevUhia.SetServiceCategoryId(item.updateConsAndDevUHIABasicDataDto.ServiceCategoryId);
                            consAndDevUhia.SetServiceSubCategoryId(item.updateConsAndDevUHIABasicDataDto.ServiceSubCategoryId);
                            consAndDevUhia.SetDataEffectiveDateFrom(item.updateConsAndDevUHIABasicDataDto.DataEffectiveDateFrom);
                            consAndDevUhia.SetDataEffectiveDateTo(item.updateConsAndDevUHIABasicDataDto.DataEffectiveDateTo);
                            consAndDevUhia.SetModifiedBy(_identityProvider.GetUserName());
                            consAndDevUhia.SetModifiedOn();

                            // prepare model to update and soft delete Item Prices
                            for (int i = 0; i < consAndDevUhia.ItemListPrices.Count; i++)
                            {
                                var itemPrice = item.updateConsAndDevUHIAPriceDto.ItemListPrices.Where(x => x.Id == consAndDevUhia.ItemListPrices[i].Id).FirstOrDefault();
                                if (itemPrice == null)
                                {
                                    continue; // do not apply soft delete now

                                    //serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                                    //serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                                }
                                else
                                {
                                    consAndDevUhia.ItemListPrices[i].SetPrice(itemPrice.Price);
                                    consAndDevUhia.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                                    consAndDevUhia.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                                    consAndDevUhia.ItemListPrices[i].SetModifiedBy(userId);
                                }
                                consAndDevUhia.ItemListPrices[i].SetModifiedOn();
                            }

                            // prepare model to add new Item Prices
                            var addItemPrices = item.updateConsAndDevUHIAPriceDto.ItemListPrices.Where(x => x.Id == 0).ToList();
                            foreach (var addedItem in addItemPrices)
                            {
                                var itemListPrice = addedItem.ToItemListPrice(userId, tenantId);
                                consAndDevUhia.ItemListPrices.Add(itemListPrice);
                            }

                            await consAndDevUhia.Update(_consumablesAndDevicesUHIARepository, _validationEngine);
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
