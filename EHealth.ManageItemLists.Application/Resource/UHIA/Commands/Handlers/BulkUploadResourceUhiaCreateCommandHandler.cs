using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Handlers
{
    public class BulkUploadResourceUhiaCreateCommandHandler : IRequestHandler<BulkUploadResourceUhiaCreateCommand, byte[]?>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository ;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IPriceUnitRepository _priceUnitRepository;
        private readonly IExcelService _excelService;
        public BulkUploadResourceUhiaCreateCommandHandler(IResourceUHIARepository resourceUHIARepository,
            IValidationEngine validationEngine,
        IIdentityProvider identityProvider,
        ICategoriesRepository categoriesRepository,
        ISubCategoriesRepository subCategoriesRepository,
        IItemListRepository itemListsRepository,
        IPriceUnitRepository priceUnitRepository,
        IExcelService excelService)
        {
            _resourceUHIARepository = resourceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _categoriesRepository = categoriesRepository;
            _subCategoriesRepository = subCategoriesRepository;
            _itemListsRepository = itemListsRepository;
            _priceUnitRepository = priceUnitRepository;
            _excelService = excelService;
        }
        public async Task<byte[]?> Handle(BulkUploadResourceUhiaCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<ResourceUhiaBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await ResourceUHIA.IsItemListBusy(_resourceUHIARepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var categories = await Domain.Categories.Category.Search(_categoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var subcategories = await Domain.Sub_Categories.SubCategory.Search(_subCategoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var priceUnits = await PriceUnit.Search(_priceUnitRepository, f => f.IsDeleted == false, 1, 1, false);
               
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var catName = _excelService.GetCell(worksheet, row, (ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index))?.ToString()?.Trim();
                    var catId = categories.Data.FirstOrDefault(c => c.CategoryEn == catName).Id;

                    var subCatName = _excelService.GetCell(worksheet, row, (ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index))?.ToString()?.Trim();
                    var subCatId = subcategories.Data.FirstOrDefault(s => s.SubCategoryEn == subCatName).Id;

                   

                    var itemListPrices = new List<UpdateResourceItemPriceDto>();
                    UpdateResourceItemPriceDto? updateReourcePriceDto = null;


                    if (_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim())) &&
                        _excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "PriceUnit").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "PriceUnit").Index))?.ToString().Trim())) &&
                         _excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index))?.ToString().Trim())))
                    {
                        var priceUnitName = _excelService.GetCell(worksheet, row, (ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "PriceUnit").Index))?.ToString()?.Trim();
                        var priceUnitId = priceUnits.Data.FirstOrDefault(s => s.NameEN == priceUnitName).Id;

                        updateReourcePriceDto = new UpdateResourceItemPriceDto()
                        {
                            Id = (_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3) == null ||
                            string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim())) ? 0
                            : int.Parse(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim()),

                            Price = double.Parse((_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim()),
                            PriceUnitId=priceUnitId,
                            EffectiveDateFrom = Convert.ToDateTime((_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index)).ToString().Trim()).Date,
                            EffectiveDateTo = (_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index) == null
                            || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim())) ? null
                            : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        };
                        itemListPrices.Add(updateReourcePriceDto);
                    }


                    var updateResourceUHIABasicDataDto = new UpdateResourceUHIABasicDataDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index)?.ToString().Trim(),
                        DescriptorAr = _excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index)?.ToString().Trim(),
                        DescriptorEn = _excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index)?.ToString().Trim(),
                        CategoryId = catId,
                        SubCategoryId = subCatId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };


                    var resourceUhiaBulkUploadDto = new ResourceUhiaBulkUploadDto
                    { 
                        updateResourceUHIABasicDataDto= updateResourceUHIABasicDataDto,
                        updateResourceUHIAPriceDto = new UpdateResourceUHIAPriceDto
                        {
                            ResourceItemPrices = itemListPrices,
                            ResourceUHIAId = (_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim()),
                        },
                        RowNumber = row
                    };
                    #endregion
                    if (updateResourceUHIABasicDataDto.Id == Guid.Empty)
                    {
                        var consAndDevUhia = updateResourceUHIABasicDataDto.ToResourceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await consAndDevUhia.ValidateObjectForBulkUpload(_resourceUHIARepository, _validationEngine);
                        foreach (var item in resourceUhiaBulkUploadDto.updateResourceUHIAPriceDto.ResourceItemPrices)
                        {
                            var itemListPrice = item.ToResourceItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
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

                        resourceUhiaBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        resourceUhiaBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var reourceUhia = await ResourceUHIA.Get(updateResourceUHIABasicDataDto.Id, _resourceUHIARepository);

                        if (reourceUhia is null)
                        {
                            errors += "resourceUhia with resourceUhia not exist." + "\r\n";
                        }
                        else
                        {
                            reourceUhia = updateResourceUHIABasicDataDto.ToResourceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await reourceUhia.ValidateObjectForBulkUpload(_resourceUHIARepository, _validationEngine);

                            var notDeletedItems = reourceUhia.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                            foreach (var item in notDeletedItems)
                            {
                                if (item.EffectiveDateFrom.Date < updateResourceUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateResourceUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateResourceUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateResourceUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                    break;
                                }
                            }

                            foreach (var item in resourceUhiaBulkUploadDto.updateResourceUHIAPriceDto.ResourceItemPrices)
                            {
                                var itemListPrice = item.ToResourceItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                var itemErrors = _validationEngine.Validate(itemListPrice, false);
                                if (itemErrors != null)
                                {
                                    foreach (var error in itemErrors)
                                    {
                                        errors += error.ErrorMessage + "\r\n";
                                    }
                                }

                                if (item.EffectiveDateFrom.Date < updateResourceUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateResourceUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateResourceUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateResourceUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                }
                                if (item.Id != 0)
                                {
                                    notDeletedItems = notDeletedItems.Where(x => x.Id != item.Id).ToList();
                                }
                                notDeletedItems.Add(item.ToResourceItemPrice(_identityProvider.GetUserId(), _identityProvider.GetTenantId()));
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
                        resourceUhiaBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        resourceUhiaBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(resourceUhiaBulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index, "Duplicated EHealthCode");
                    }


                    if (!(resourceUhiaBulkUploadDto.errors.Contains("Duplicated ShortDescAr")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index, "Duplicated ShortDescAr");
                    }

                    if (!(resourceUhiaBulkUploadDto.errors.Contains("Duplicated ShortDescEn")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index, "Duplicated ShortDescEn");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        resourceUhiaBulkUploadDto.IsValid = false;
                        resourceUhiaBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!resourceUhiaBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue(resourceUhiaBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue("");
                    }
                    list.Add(resourceUhiaBulkUploadDto);
                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.updateResourceUHIABasicDataDto.Id == Guid.Empty)
                        {
                            var resourceUhiaWithoutId = item.updateResourceUHIABasicDataDto.ToResourceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await resourceUhiaWithoutId.Create(_resourceUHIARepository, _validationEngine);

                            var resourceUhia = await ResourceUHIA.Get(resourceUhiaWithoutId.Id, _resourceUHIARepository);

                            foreach (var price in item.updateResourceUHIAPriceDto.ResourceItemPrices)
                            {
                                var itemListPrice = price.ToResourceItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                resourceUhia.ItemListPrices.Add(itemListPrice);
                            }
                            await resourceUhia.Update(_resourceUHIARepository, _validationEngine,userId);
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                            CellId.SetCellValue(resourceUhia?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);
                            var cellpriceId = worksheet.GetRow(item.RowNumber).CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3);
                            cellpriceId.SetCellValue(resourceUhia?.ItemListPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()?.ToString());
                            #endregion
                        }
                        else
                        {
                            var resourceUhia = await ResourceUHIA.Get(item.updateResourceUHIABasicDataDto.Id, _resourceUHIARepository);
                           resourceUhia.SetEHealthCode(item.updateResourceUHIABasicDataDto.EHealthCode);
                           resourceUhia.SetDescriptorAr(item.updateResourceUHIABasicDataDto.DescriptorAr);
                           resourceUhia.SetDescriptorEn(item.updateResourceUHIABasicDataDto.DescriptorEn);
                           resourceUhia.SetCategoryId(item.updateResourceUHIABasicDataDto.CategoryId);
                           resourceUhia.SetSubCategoryId(item.updateResourceUHIABasicDataDto.SubCategoryId);
                           resourceUhia.SetDataEffectiveDateFrom(item.updateResourceUHIABasicDataDto.DataEffectiveDateFrom);
                           resourceUhia.SetDataEffectiveDateTo(item.updateResourceUHIABasicDataDto.DataEffectiveDateTo);
                           resourceUhia.SetModifiedBy(_identityProvider.GetUserName());
                            resourceUhia.SetModifiedOn();

                            // prepare model to update and soft delete Item Prices
                            for (int i = 0; i < resourceUhia.ItemListPrices.Count; i++)
                            {
                                var itemPrice = item.updateResourceUHIAPriceDto.ResourceItemPrices.Where(x => x.Id == resourceUhia.ItemListPrices[i].Id).FirstOrDefault();
                                if (itemPrice == null)
                                {
                                    continue; // do not apply soft delete now

                                    //serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                                    //serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                                }
                                else
                                {
                                    resourceUhia.ItemListPrices[i].SetPrice(itemPrice.Price);
                                    resourceUhia.ItemListPrices[i].SetPriceUnitId(itemPrice.PriceUnitId);
                                    resourceUhia.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                                    resourceUhia.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                                    resourceUhia.ItemListPrices[i].SetModifiedBy(userId);
                                }
                                resourceUhia.ItemListPrices[i].SetModifiedOn();
                            }

                            // prepare model to add new Item Prices
                            var addItemPrices = item.updateResourceUHIAPriceDto.ResourceItemPrices.Where(x => x.Id == 0).ToList();
                            foreach (var addedItem in addItemPrices)
                            {
                                var itemListPrice = addedItem.ToResourceItemPrice(userId, tenantId);
                                resourceUhia.ItemListPrices.Add(itemListPrice);
                            }

                            await resourceUhia.Update(_resourceUHIARepository, _validationEngine,userId);
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
