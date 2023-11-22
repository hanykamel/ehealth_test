using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Handler
{
    public class BulkUploaDrFeesCreateCommandHandler : IRequestHandler<BulkUploadDrFeesCreateCommand, byte[]?>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly IPackageComplexityClassificationRepository _packageComplexityClassificationRepository;
        private readonly IUnitDOFRepository _unitDOFRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;
        public BulkUploaDrFeesCreateCommandHandler(IDoctorFeesUHIARepository doctorFeesUHIARepositor,
             IValidationEngine validationEngine,
        IIdentityProvider identityProvider,
        IPackageComplexityClassificationRepository packageComplexityClassificationRepository,
        IUnitDOFRepository unitDOFRepository,
        IItemListRepository itemListsRepository,
        IExcelService excelService)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepositor;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _itemListsRepository = itemListsRepository;
            _excelService = excelService;
            _packageComplexityClassificationRepository = packageComplexityClassificationRepository;
            _unitDOFRepository = unitDOFRepository;
        }
        public async Task<byte[]?> Handle(BulkUploadDrFeesCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<DrFeesBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await DoctorFeesUHIA.IsItemListBusy(_doctorFeesUHIARepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var packageCompexity = await PackageComplexityClassification.Search(_packageComplexityClassificationRepository, f => f.IsDeleted == false, 1, 1, false);
                var unitDrFees = await UnitDOF.Search(_unitDOFRepository, f => f.IsDeleted == false, 1, 1, false);
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var packageCompexityName = _excelService.GetCell(worksheet, row, (DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "PackageComplexityClassification").Index))?.ToString()?.Trim();
                    var packageCompexityId = packageCompexity.Data.FirstOrDefault(c => c.ComplexityEn == packageCompexityName).Id;

                    var drFeesName = _excelService.GetCell(worksheet, row, (DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UnitOfDoctorFees").Index))?.ToString()?.Trim();
                    var drFeesId = unitDrFees.Data.FirstOrDefault(c => c.NameEN == drFeesName).Id;

                    var DrFeesitemListPrices = new List<UpdateDoctorFeesItemPriceDto>();
                    UpdateDoctorFeesItemPriceDto? updateDrFeesItemListPriceDto = null;


                    if (_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DoctorFees").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DoctorFees").Index))?.ToString().Trim())) &&
                    _excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UnitOfDoctorFees").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UnitOfDoctorFees").Index))?.ToString().Trim())) &&
                         _excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index))?.ToString().Trim())))
                    {
                        updateDrFeesItemListPriceDto = new UpdateDoctorFeesItemPriceDto()
                        {
                            Id = (_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3) == null ||
                            string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim())) ? 0
                            : int.Parse(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim()),

                            DoctorFees = double.Parse((_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DoctorFees").Index))?.ToString().Trim()),
                            UnitOfDoctorFeesId = drFeesId,
                            EffectiveDateFrom = Convert.ToDateTime((_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index)).ToString().Trim()).Date,
                            EffectiveDateTo = (_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index) == null
                            || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim())) ? null
                            : Convert.ToDateTime(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        };


                        DrFeesitemListPrices.Add(updateDrFeesItemListPriceDto);
                    }


                    var updateDoctoerFeesUHIABasicDataDto = new UpdateDoctoerFeesUHIABasicDataDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index)?.ToString().Trim(),
                        DescriptorAr = _excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index)?.ToString().Trim(),
                        DescriptorEn = _excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index)?.ToString().Trim(),
                        PackageCompexityClassificationId=packageCompexityId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };


                    var drFeesBulkUploadDto = new DrFeesBulkUploadDto
                    {
                        updateDoctoerFeesUHIABasicDataDto = updateDoctoerFeesUHIABasicDataDto,
                        updateDoctorFeesUHIAPriceDto = new UpdateDoctorFeesUHIAPriceDto
                        {
                            ItemListPrices = DrFeesitemListPrices,
                            DoctorFeesUHIAId = (_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim()),
                        },
                        RowNumber = row
                    };
                    #endregion
                    if (updateDoctoerFeesUHIABasicDataDto.Id == Guid.Empty)
                    {
                        var drFeesUhiaUhia = updateDoctoerFeesUHIABasicDataDto.ToDrFeesUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await drFeesUhiaUhia.ValidateObjectForBulkUpload(_doctorFeesUHIARepository, _validationEngine);
                        foreach (var item in drFeesBulkUploadDto.updateDoctorFeesUHIAPriceDto.ItemListPrices)
                        {
                            var itemListPrice = item.ToDrFeesItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            var itemErrors = _validationEngine.Validate(itemListPrice, false);
                            if (itemErrors != null)
                            {
                                foreach (var error in itemErrors)
                                {
                                    errors += error.ErrorMessage + "\r\n";
                                }
                            }

                            if (item.EffectiveDateFrom.Date < drFeesUhiaUhia.DataEffectiveDateFrom.Date ||
                                (item.EffectiveDateTo.HasValue && drFeesUhiaUhia.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > drFeesUhiaUhia.DataEffectiveDateTo.Value.Date) ||
                                ((!item.EffectiveDateTo.HasValue) && drFeesUhiaUhia.DataEffectiveDateTo.HasValue))
                            {
                                errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                            }
                        }

                        drFeesBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        drFeesBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var drFeesUhia = await DoctorFeesUHIA.Get(updateDoctoerFeesUHIABasicDataDto.Id, _doctorFeesUHIARepository);

                        if (drFeesUhia is null)
                        {
                            errors += "DoctorFeesUHIA with DoctorFeesUHIAId not exist." + "\r\n";
                        }
                        else
                        {
                            drFeesUhia = updateDoctoerFeesUHIABasicDataDto.ToDrFeesUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await drFeesUhia.ValidateObjectForBulkUpload(_doctorFeesUHIARepository, _validationEngine);

                            var notDeletedItems = drFeesUhia.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                            foreach (var item in notDeletedItems)
                            {
                                if (item.EffectiveDateFrom.Date < updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                    break;
                                }
                            }

                            foreach (var item in drFeesBulkUploadDto.updateDoctorFeesUHIAPriceDto.ItemListPrices)
                            {
                                var itemListPrice = item.ToDrFeesItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                var itemErrors = _validationEngine.Validate(itemListPrice, false);
                                if (itemErrors != null)
                                {
                                    foreach (var error in itemErrors)
                                    {
                                        errors += error.ErrorMessage + "\r\n";
                                    }
                                }

                                if (item.EffectiveDateFrom.Date < updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                }
                                if (item.Id != 0)
                                {
                                    notDeletedItems = notDeletedItems.Where(x => x.Id != item.Id).ToList();
                                }
                                notDeletedItems.Add(item.ToDrFeesItemPrice(_identityProvider.GetUserId(), _identityProvider.GetTenantId()));
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
                        drFeesBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        drFeesBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(drFeesBulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index, "Duplicated EHealthCode");
                    }


                    if (!(drFeesBulkUploadDto.errors.Contains("Duplicated ShortDescAr")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index, "Duplicated ShortDescAr");
                    }

                    if (!(drFeesBulkUploadDto.errors.Contains("Duplicated ShortDescEn")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index, "Duplicated ShortDescEn");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        drFeesBulkUploadDto.IsValid = false;
                        drFeesBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!drFeesBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue(drFeesBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue("");
                    }
                    list.Add(drFeesBulkUploadDto);

                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.updateDoctoerFeesUHIABasicDataDto.Id == Guid.Empty)
                        {
                            var drFeesUhiaWithoutId = item.updateDoctoerFeesUHIABasicDataDto.ToDrFeesUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await drFeesUhiaWithoutId.Create(_doctorFeesUHIARepository, _validationEngine);

                            var drFeesUhia = await DoctorFeesUHIA.Get(drFeesUhiaWithoutId.Id, _doctorFeesUHIARepository);

                            foreach (var price in item.updateDoctorFeesUHIAPriceDto.ItemListPrices)
                            {
                                var itemListPrice = price.ToDrFeesItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                drFeesUhia.ItemListPrices.Add(itemListPrice);
                            }
                            await drFeesUhia.Update(_doctorFeesUHIARepository, _validationEngine,userId);
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                            CellId.SetCellValue(drFeesUhia?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);
                            var cellpriceId = worksheet.GetRow(item.RowNumber).CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3);
                            cellpriceId.SetCellValue(drFeesUhia?.ItemListPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()?.ToString());
                            #endregion
                        }
                        else
                        {
                            var drFeesUhia = await DoctorFeesUHIA.Get(item.updateDoctoerFeesUHIABasicDataDto.Id, _doctorFeesUHIARepository);
                            drFeesUhia.SetEHealthCode(item.updateDoctoerFeesUHIABasicDataDto.EHealthCode);
                            drFeesUhia.SetDescriptorAr(item.updateDoctoerFeesUHIABasicDataDto.DescriptorAr);
                            drFeesUhia.SetDescriptorEn(item.updateDoctoerFeesUHIABasicDataDto.DescriptorEn);
                            drFeesUhia.SetPackageComplexityClassificationId(item.updateDoctoerFeesUHIABasicDataDto.PackageCompexityClassificationId);
                            drFeesUhia.SetDataEffectiveDateFrom(item.updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateFrom);
                            drFeesUhia.SetDataEffectiveDateTo(item.updateDoctoerFeesUHIABasicDataDto.DataEffectiveDateTo);
                            drFeesUhia.SetModifiedBy(_identityProvider.GetUserName());
                            drFeesUhia.SetModifiedOn();

                            // prepare model to update and soft delete Item Prices
                            for (int i = 0; i < drFeesUhia.ItemListPrices.Count; i++)
                            {
                                var itemPrice = item.updateDoctorFeesUHIAPriceDto.ItemListPrices.Where(x => x.Id == drFeesUhia.ItemListPrices[i].Id).FirstOrDefault();
                                if (itemPrice == null)
                                {
                                    continue; // do not apply soft delete now

                                    //serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                                    //serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                                }
                                else
                                {
                                    drFeesUhia.ItemListPrices[i].SetDoctorFees(itemPrice.DoctorFees);
                                    drFeesUhia.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                                    drFeesUhia.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                                    drFeesUhia.ItemListPrices[i].SetModifiedBy(userId);
                                }
                                drFeesUhia.ItemListPrices[i].SetModifiedOn();
                            }

                            // prepare model to add new Item Prices
                            var addItemPrices = item.updateDoctorFeesUHIAPriceDto.ItemListPrices.Where(x => x.Id == 0).ToList();
                            foreach (var addedItem in addItemPrices)
                            {
                                var itemListPrice = addedItem.ToDrFeesItemPrice(userId, tenantId);
                                drFeesUhia.ItemListPrices.Add(itemListPrice);
                            }

                            await drFeesUhia.Update(_doctorFeesUHIARepository, _validationEngine,userId);
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
