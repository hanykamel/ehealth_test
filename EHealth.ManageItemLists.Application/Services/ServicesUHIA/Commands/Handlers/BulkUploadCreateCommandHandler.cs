using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Handlers
{
    public class BulkUploadCreateCommandHandler : IRequestHandler<BulkUploadCreateCommand, byte[]?>
    {
        //get
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;

        public BulkUploadCreateCommandHandler(IServiceUHIARepository serviceUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider,
        ICategoriesRepository categoriesRepository,
        ISubCategoriesRepository subCategoriesRepository,
        IItemListRepository itemListsRepository,
        IExcelService excelService)
        {
            _serviceUHIARepository = serviceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _categoriesRepository = categoriesRepository;
            _subCategoriesRepository = subCategoriesRepository;
            _itemListsRepository = itemListsRepository;
            _excelService = excelService;
        }
        public async Task<byte[]?> Handle(BulkUploadCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<BulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);
     
            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await ServiceUHIA.IsItemListBusy(_serviceUHIARepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(14);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(14);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var categories = await Domain.Categories.Category.Search(_categoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var subcategories = await Domain.Sub_Categories.SubCategory.Search(_subCategoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var catName = _excelService.GetCell(worksheet, row, (ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index))?.ToString()?.Trim();
                    var catId = categories.Data.FirstOrDefault(c => c.CategoryEn == catName).Id;

                    var subCatName = _excelService.GetCell(worksheet, row, (ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index))?.ToString()?.Trim();
                    var subCatId = subcategories.Data.FirstOrDefault(s => s.SubCategoryEn == subCatName).Id;


                    var itemListPrices = new List<UpdateItemListPriceDto>();
                    UpdateItemListPriceDto? updateItemListPriceDto = null;


                    if (_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim())) &&
                         _excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index))?.ToString().Trim())))
                    {
                        updateItemListPriceDto = new UpdateItemListPriceDto()
                        {
                            Id = (_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3) == null ||
                            string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim())) ? 0
                            : int.Parse(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim()),

                            Price = double.Parse((_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim()),
                            EffectiveDateFrom = Convert.ToDateTime((_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index)).ToString().Trim()).Date,
                            EffectiveDateTo = (_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index) == null
                            || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim())) ? null
                            : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        };
                        itemListPrices.Add(updateItemListPriceDto);
                    }


                    var updateServicesUHIABasicDataDto = new UpdateServicesUHIABasicDataDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index)?.ToString().Trim(),
                        UHIAId = _excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UHIAId").Index)?.ToString().Trim(),
                        ShortDescAr = _excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescAr").Index)?.ToString().Trim(),
                        ShortDescEn = _excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescEn").Index)?.ToString().Trim(),
                        ServiceCategoryId = catId,
                        ServiceSubCategoryId = subCatId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };


                    var bulkUploadDto = new BulkUploadDto
                    {
                        UpdateServicesUHIABasicDataDto = updateServicesUHIABasicDataDto,
                        UpdateServicesUHIAPriceDto = new UpdateServicesUHIAPriceDto
                        {
                            ItemListPrices = itemListPrices,
                            ServiceUHIAId = (_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim()),
                        },
                        RowNumber = row
                    };
                    #endregion
                    if (updateServicesUHIABasicDataDto.Id == Guid.Empty)
                    {
                        var serviceUHIA = updateServicesUHIABasicDataDto.ToServiceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await serviceUHIA.ValidateObjectForBulkUpload(_serviceUHIARepository, _validationEngine);

                        foreach (var item in bulkUploadDto.UpdateServicesUHIAPriceDto.ItemListPrices)
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

                            if (item.EffectiveDateFrom.Date < serviceUHIA.DataEffectiveDateFrom.Date ||
                                (item.EffectiveDateTo.HasValue && serviceUHIA.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > serviceUHIA.DataEffectiveDateTo.Value.Date) ||
                                ((!item.EffectiveDateTo.HasValue) && serviceUHIA.DataEffectiveDateTo.HasValue))
                            {
                                errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                            }
                        }

                        bulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        bulkUploadDto.errors = errors;
                    }
                    else
                    {
                        var serviceUHIA = await ServiceUHIA.Get(updateServicesUHIABasicDataDto.Id, _serviceUHIARepository);

                        if (serviceUHIA is null)
                        {
                            errors += "ServiceUHIA with ServiceUHIAId not exist." + "\r\n";
                        }
                        else
                        {
                            serviceUHIA = updateServicesUHIABasicDataDto.ToServiceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await serviceUHIA.ValidateObjectForBulkUpload(_serviceUHIARepository, _validationEngine);

                            var notDeletedItems = serviceUHIA.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                            foreach (var item in notDeletedItems)
                            {
                                if (item.EffectiveDateFrom.Date < updateServicesUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateServicesUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateServicesUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateServicesUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                    break;
                                }
                            }

                            foreach (var item in bulkUploadDto.UpdateServicesUHIAPriceDto.ItemListPrices)
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

                                if (item.EffectiveDateFrom.Date < updateServicesUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateServicesUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateServicesUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateServicesUHIABasicDataDto.DataEffectiveDateTo.HasValue))
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
                        bulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        bulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(bulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index, "Duplicated EHealthCode");
                    }

                    if (!(bulkUploadDto.errors.Contains("Duplicated UHIAId")))
                    {

                        CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UHIAId").Index, "Duplicated UHIAId");
                    }

                    if (!(bulkUploadDto.errors.Contains("Duplicated ShortDescAr")))
                    {

                        CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescAr").Index, "Duplicated ShortDescAr");
                    }

                    if (!(bulkUploadDto.errors.Contains("Duplicated ShortDescEn")))
                    {

                        CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescEn").Index, "Duplicated ShortDescEn");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        bulkUploadDto.IsValid = false;
                        bulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!bulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue(bulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue("");


                    }


                    list.Add(bulkUploadDto);

                }

              

                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.UpdateServicesUHIABasicDataDto.Id == Guid.Empty)
                        {
                            var serviceUHIAWithoutId = item.UpdateServicesUHIABasicDataDto.ToServiceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await serviceUHIAWithoutId.Create(_serviceUHIARepository, _validationEngine);

                            var serviceUHIA = await ServiceUHIA.Get(serviceUHIAWithoutId.Id, _serviceUHIARepository);

                            foreach (var price in item.UpdateServicesUHIAPriceDto.ItemListPrices)
                            {
                                var itemListPrice = price.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                serviceUHIA.ItemListPrices.Add(itemListPrice);
                            }
                            await serviceUHIA.Update(_serviceUHIARepository, _validationEngine);
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                            CellId.SetCellValue(serviceUHIA?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);
                            var cellpriceId = worksheet.GetRow(item.RowNumber).CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3);
                            cellpriceId.SetCellValue(serviceUHIA?.ItemListPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()?.ToString());
                            #endregion
                        }
                        else
                        {
                            var serviceUHIA = await ServiceUHIA.Get(item.UpdateServicesUHIABasicDataDto.Id, _serviceUHIARepository);
                            serviceUHIA.SetEHealthCode(item.UpdateServicesUHIABasicDataDto.EHealthCode);
                            serviceUHIA.SetUHIAId(item.UpdateServicesUHIABasicDataDto.UHIAId);
                            serviceUHIA.SetShortDescAr(item.UpdateServicesUHIABasicDataDto.ShortDescAr);
                            serviceUHIA.SetShortDescEn(item.UpdateServicesUHIABasicDataDto.ShortDescEn);
                            serviceUHIA.SetServiceCategoryId(item.UpdateServicesUHIABasicDataDto.ServiceCategoryId);
                            serviceUHIA.SetServiceSubCategoryId(item.UpdateServicesUHIABasicDataDto.ServiceSubCategoryId);
                            serviceUHIA.SetDataEffectiveDateFrom(item.UpdateServicesUHIABasicDataDto.DataEffectiveDateFrom);
                            serviceUHIA.SetDataEffectiveDateTo(item.UpdateServicesUHIABasicDataDto.DataEffectiveDateTo);
                            serviceUHIA.SetModifiedBy(_identityProvider.GetUserName());
                            serviceUHIA.SetModifiedOn();

                            // prepare model to update and soft delete Item Prices
                            for (int i = 0; i < serviceUHIA.ItemListPrices.Count; i++)
                            {
                                var itemPrice = item.UpdateServicesUHIAPriceDto.ItemListPrices.Where(x => x.Id == serviceUHIA.ItemListPrices[i].Id).FirstOrDefault();
                                if (itemPrice == null)
                                {
                                    continue; // do not apply soft delete now

                                    //serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                                    //serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                                }
                                else
                                {
                                    serviceUHIA.ItemListPrices[i].SetPrice(itemPrice.Price);
                                    serviceUHIA.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                                    serviceUHIA.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                                    serviceUHIA.ItemListPrices[i].SetModifiedBy(userId);
                                }
                                serviceUHIA.ItemListPrices[i].SetModifiedOn();
                            }

                            // prepare model to add new Item Prices
                            var addItemPrices = item.UpdateServicesUHIAPriceDto.ItemListPrices.Where(x => x.Id == 0).ToList();
                            foreach (var addedItem in addItemPrices)
                            {
                                var itemListPrice = addedItem.ToItemListPrice(userId, tenantId);
                                serviceUHIA.ItemListPrices.Add(itemListPrice);
                            }

                            await serviceUHIA.Update(_serviceUHIARepository, _validationEngine);
                          
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

        private void CheckDupllicatesInFile(ISheet worksheet, int rowCounts, ref string dublicatedProperties, int row, int cellIndex, string dublicatedValue)
        {
            for (int i = 1; i <= rowCounts; i++)
            {
                if (i == row)
                {
                    continue;
                }
                if (_excelService.GetCell(worksheet, row, cellIndex)?.ToString() == _excelService.GetCell(worksheet, i, cellIndex)?.ToString())
                {
                    dublicatedProperties += dublicatedValue;/* "Duplicated UHIAId,";*/
                    break;
                }

            }
        }
    }
}
