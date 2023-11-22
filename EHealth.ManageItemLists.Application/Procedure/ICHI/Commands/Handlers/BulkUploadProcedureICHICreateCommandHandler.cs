using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands;
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
using EHealth.ManageItemLists.Infrastructure.Repositories.Lookups;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.RegistrationTypes;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Handlers
{
    public class BulkUploadProcedureICHICreateCommandHandler : IRequestHandler<BulkUploadProcedureICHICreateCommand, byte[]?>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly ILocalSpecialtyDepartmentsRepository _localSpecialtyDepartmentsRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;
        public BulkUploadProcedureICHICreateCommandHandler(IProcedureICHIRepository procedureICHIRepository,
          IValidationEngine validationEngine,
         IIdentityProvider identityProvider,
         ICategoriesRepository categoriesRepository,
         ISubCategoriesRepository subCategoriesRepository,
         ILocalSpecialtyDepartmentsRepository localSpecialtyDepartmentsRepository,
         IItemListRepository itemListsRepository,
         IExcelService excelService)
        {
            _procedureICHIRepository = procedureICHIRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _categoriesRepository = categoriesRepository;
            _subCategoriesRepository = subCategoriesRepository;
            _localSpecialtyDepartmentsRepository = localSpecialtyDepartmentsRepository;
            _itemListsRepository = itemListsRepository;
            _excelService = excelService;
             
        }
        public async Task<byte[]?> Handle(BulkUploadProcedureICHICreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<ProcedureICHIBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await ProcedureICHI.IsItemListBusy(_procedureICHIRepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var categories = await Domain.Categories.Category.Search(_categoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var subcategories = await Domain.Sub_Categories.SubCategory.Search(_subCategoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var localSpec = await LocalSpecialtyDepartment.Search(_localSpecialtyDepartmentsRepository, f => f.IsDeleted == false, 1, 1, false);
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var catName = _excelService.GetCell(worksheet, row, (ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index))?.ToString()?.Trim();
                    var catId = categories.Data.FirstOrDefault(c => c.CategoryEn == catName).Id;

                    var subCatName = _excelService.GetCell(worksheet, row, (ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index))?.ToString()?.Trim();
                    var subCatId = subcategories.Data.FirstOrDefault(s => s.SubCategoryEn == subCatName).Id;

                    var localSpecName = _excelService.GetCell(worksheet, row, (ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "LocalSpecialtyDepartment").Index))?.ToString()?.Trim();
                    var localSpecId = localSpec.Data.FirstOrDefault(c => (!string.IsNullOrEmpty(localSpecName) ? c.LocalSpecialityENG.ToLower().Contains(localSpecName.ToLower()) : true)).Id;

                    var itemListPrices = new List<UpdateItemListPriceDto>();
                    UpdateItemListPriceDto? updateItemListPriceDto = null;


                    if (_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim())) &&
                         _excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index))?.ToString().Trim())))
                    {
                        updateItemListPriceDto = new UpdateItemListPriceDto()
                        {
                            Id = (_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3) == null ||
                            string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim())) ? 0
                            : int.Parse(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim()),

                            Price = double.Parse((_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index))?.ToString().Trim()),
                            EffectiveDateFrom = Convert.ToDateTime((_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index)).ToString().Trim()).Date,
                            EffectiveDateTo = (_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index) == null
                            || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim())) ? null
                            : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        };
                        itemListPrices.Add(updateItemListPriceDto);
                    }


                    var updateProcedureICHIBasicDataDto = new UpdateProcedureICHIBasicDataDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index)?.ToString().Trim(),
                        UHIAId = _excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UHIAId").Index)?.ToString().Trim(),
                        TitleAr = _excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "TitleAr").Index)?.ToString().Trim(),
                        TitleEn = _excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "TitleEn").Index)?.ToString().Trim(),
                        ServiceCategoryId = catId,
                        ServiceSubCategoryId = subCatId,
                        LocalSpecialtyDepartmentId = localSpecId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };


                    var procedureICHIBulkUploadDto = new ProcedureICHIBulkUploadDto
                    {
                        updateProcedureICHIBasicDataDto = updateProcedureICHIBasicDataDto,
                        updateProcedureICHIPriceDto = new UpdateProcedureICHIPriceDto
                        {
                            ItemListPrices = itemListPrices,
                            ProcedureICHIId = (_excelService.GetCell(worksheet, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim()),
                        },
                        RowNumber = row
                    };
                    #endregion
                    if (updateProcedureICHIBasicDataDto.Id == Guid.Empty)
                    {
                        var procedureICHI = updateProcedureICHIBasicDataDto.ToProcedureICHI(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await procedureICHI.ValidateObjectForBulkUpload(_procedureICHIRepository, _validationEngine);
                        foreach (var item in procedureICHIBulkUploadDto.updateProcedureICHIPriceDto.ItemListPrices)
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

                            if (item.EffectiveDateFrom.Date < procedureICHI.DataEffectiveDateFrom.Date ||
                                (item.EffectiveDateTo.HasValue && procedureICHI.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > procedureICHI.DataEffectiveDateTo.Value.Date) ||
                                ((!item.EffectiveDateTo.HasValue) && procedureICHI.DataEffectiveDateTo.HasValue))
                            {
                                errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                            }
                        }

                        procedureICHIBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        procedureICHIBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var procedureICHI = await ProcedureICHI.Get(updateProcedureICHIBasicDataDto.Id, _procedureICHIRepository);

                        if (procedureICHI is null)
                        {
                            errors += "ProcedureIchi with ProcedureIchiId not exist." + "\r\n";
                        }
                        else
                        {
                            procedureICHI = updateProcedureICHIBasicDataDto.ToProcedureICHI(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await procedureICHI.ValidateObjectForBulkUpload(_procedureICHIRepository, _validationEngine);

                            var notDeletedItems = procedureICHI.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                            foreach (var item in notDeletedItems)
                            {
                                if (item.EffectiveDateFrom.Date < updateProcedureICHIBasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateProcedureICHIBasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateProcedureICHIBasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateProcedureICHIBasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                    break;
                                }
                            }

                            foreach (var item in procedureICHIBulkUploadDto.updateProcedureICHIPriceDto.ItemListPrices)
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

                                if (item.EffectiveDateFrom.Date < updateProcedureICHIBasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateProcedureICHIBasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateProcedureICHIBasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateProcedureICHIBasicDataDto.DataEffectiveDateTo.HasValue))
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
                        procedureICHIBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        procedureICHIBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(procedureICHIBulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index, "Duplicated EHealthCode");
                    }

                    if (!(procedureICHIBulkUploadDto.errors.Contains("Duplicated UHIAId")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UHIAId").Index, "Duplicated UHIAId");
                    }

                    if (!(procedureICHIBulkUploadDto.errors.Contains("Duplicated ShortDescAr")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "TitleAr").Index, "Duplicated ShortDescAr");
                    }

                    if (!(procedureICHIBulkUploadDto.errors.Contains("Duplicated ShortDescEn")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "TitleEn").Index, "Duplicated ShortDescEn");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        procedureICHIBulkUploadDto.IsValid = false;
                        procedureICHIBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!procedureICHIBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue(procedureICHIBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue("");
                    }
                    list.Add(procedureICHIBulkUploadDto);
                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.updateProcedureICHIBasicDataDto.Id == Guid.Empty)
                        {
                            var procedureICHIWithoutId = item.updateProcedureICHIBasicDataDto.ToProcedureICHI(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await procedureICHIWithoutId.Create(_procedureICHIRepository, _validationEngine);

                            var procedureICHI = await ProcedureICHI.Get(procedureICHIWithoutId.Id, _procedureICHIRepository);

                            foreach (var price in item.updateProcedureICHIPriceDto.ItemListPrices)
                            {
                                var itemListPrice = price.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                procedureICHI.ItemListPrices.Add(itemListPrice);
                            }
                            await procedureICHI.Update(_procedureICHIRepository, _validationEngine,userId);
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                            CellId.SetCellValue(procedureICHI?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);
                            var cellpriceId = worksheet.GetRow(item.RowNumber).CreateCell(ProcedureIchiTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3);
                            cellpriceId.SetCellValue(procedureICHI?.ItemListPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()?.ToString());
                            #endregion
                        }
                        else
                        {
                            var procedureICHI = await ProcedureICHI.Get(item.updateProcedureICHIBasicDataDto.Id, _procedureICHIRepository);
                            procedureICHI.SetEHealthCode(item.updateProcedureICHIBasicDataDto.EHealthCode);
                            procedureICHI.SetUHIAId(item.updateProcedureICHIBasicDataDto.UHIAId);
                            procedureICHI.SetTitleAr(item.updateProcedureICHIBasicDataDto.TitleAr);
                            procedureICHI.SetTitleEn(item.updateProcedureICHIBasicDataDto.TitleEn);
                            procedureICHI.SetLocalSpecialtyDepartmentId(item.updateProcedureICHIBasicDataDto.LocalSpecialtyDepartmentId);
                            procedureICHI.SetServiceCategoryId(item.updateProcedureICHIBasicDataDto.ServiceCategoryId);
                            procedureICHI.SetServiceSubCategoryId(item.updateProcedureICHIBasicDataDto.ServiceSubCategoryId);
                            procedureICHI.SetDataEffectiveDateFrom(item.updateProcedureICHIBasicDataDto.DataEffectiveDateFrom);
                            procedureICHI.SetDataEffectiveDateTo(item.updateProcedureICHIBasicDataDto.DataEffectiveDateTo);
                            procedureICHI.SetModifiedBy(_identityProvider.GetUserName());
                            procedureICHI.SetModifiedOn();

                            // prepare model to update and soft delete Item Prices
                            for (int i = 0; i < procedureICHI.ItemListPrices.Count; i++)
                            {
                                var itemPrice = item.updateProcedureICHIPriceDto.ItemListPrices.Where(x => x.Id == procedureICHI.ItemListPrices[i].Id).FirstOrDefault();
                                if (itemPrice == null)
                                {
                                    continue; // do not apply soft delete now

                                    //serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                                    //serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                                }
                                else
                                {
                                    procedureICHI.ItemListPrices[i].SetPrice(itemPrice.Price);
                                    procedureICHI.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                                    procedureICHI.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                                    procedureICHI.ItemListPrices[i].SetModifiedBy(userId);
                                }
                                procedureICHI.ItemListPrices[i].SetModifiedOn();
                            }

                            // prepare model to add new Item Prices
                            var addItemPrices = item.updateProcedureICHIPriceDto.ItemListPrices.Where(x => x.Id == 0).ToList();
                            foreach (var addedItem in addItemPrices)
                            {
                                var itemListPrice = addedItem.ToItemListPrice(userId, tenantId);
                                procedureICHI.ItemListPrices.Add(itemListPrice);
                            }

                            await procedureICHI.Update(_procedureICHIRepository, _validationEngine,userId);
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
