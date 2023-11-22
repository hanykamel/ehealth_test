using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.RegistrationTypes;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Handlers
{
    public class BulkUploadDrugUhiaCreateCommandHandler : IRequestHandler<BulkUploadDrugsUhiaCreateCommand, byte[]?>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IDrugsPackageTypeRepository _drugsPackageTypeRepository;
        private readonly IUnitsTypeRepository _unitsTypeRepository;
        private readonly IReimbursementCategoryRepository _reimbursementCategoryRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;
        public BulkUploadDrugUhiaCreateCommandHandler(IDrugsUHIARepository drugsUHIARepository,
            IValidationEngine validationEngine,
            IIdentityProvider identityProvider,
            IRegistrationRepository registrationRepository,
            IDrugsPackageTypeRepository drugsPackageTypeRepository,
            IUnitsTypeRepository unitsTypeRepository,
            IReimbursementCategoryRepository reimbursementCategoryRepository,
            IItemListRepository itemListRepository,
            IExcelService excelService)

        {
            _drugsUHIARepository = drugsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _registrationRepository = registrationRepository;
            _drugsPackageTypeRepository = drugsPackageTypeRepository;
            _unitsTypeRepository = unitsTypeRepository;
            _reimbursementCategoryRepository = reimbursementCategoryRepository;
            _itemListsRepository = itemListRepository;
            _excelService = excelService;
        }
        public async Task<byte[]?> Handle(BulkUploadDrugsUhiaCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<DrugUhiaBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await DrugUHIA.IsItemListBusy(_drugsUHIARepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var registrationTypes = await RegistrationType.Search(_registrationRepository, f => f.IsDeleted == false, 1, 1, false);
                var drugsPackageTypes = await DrugsPackageType.Search(_drugsPackageTypeRepository, f => f.IsDeleted == false, 1, 1, false);
                var unitTypes = await UnitsType.Search(_unitsTypeRepository, f => f.IsDeleted == false, 1, 1, false);
                var reimbursementCategories = await ReimbursementCategory.Search(_reimbursementCategoryRepository, f => f.IsDeleted == false, 1, 1, false);

                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var RegistrationTypeName = _excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "RegistrationType").Index))?.ToString()?.Trim();
                    var RegistrationTypeId = registrationTypes.Data.FirstOrDefault(c => (!string.IsNullOrEmpty(RegistrationTypeName) ? c.RegistrationTypeENG.ToLower().Contains(RegistrationTypeName.ToLower()) : true)).Id;
                   
                    var drugsPackageTypeName = _excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DrugsPackageType").Index))?.ToString()?.Trim();
                    var drugsPackageTypeId = drugsPackageTypes.Data.FirstOrDefault(s => (!string.IsNullOrEmpty(drugsPackageTypeName) ? s.NameEN.ToLower().Contains(drugsPackageTypeName.ToLower()) : true)).Id;
                    
                    var mainUnitName = _excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MainUnit").Index))?.ToString()?.Trim();
                    var mainUnitId = unitTypes.Data.FirstOrDefault(s => (!string.IsNullOrEmpty(mainUnitName) ? s.UnitEn.ToLower().Contains(mainUnitName.ToLower()) : true)).Id;
             

                    var subUnitName = _excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubUnit").Index))?.ToString()?.Trim();
                    var subUnitId = unitTypes.Data.FirstOrDefault(s => (!string.IsNullOrEmpty(subUnitName) ? s.UnitEn.ToLower().Contains(subUnitName.ToLower()) : true)).Id;
                    
                    var ReimbursementCategoryName = _excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ReimbursementCategory").Index))?.ToString()?.Trim();
                    var ReimbursementCategoryId = reimbursementCategories.Data.FirstOrDefault(s => (!string.IsNullOrEmpty(ReimbursementCategoryName) ? s.NameENG.ToLower().Contains(ReimbursementCategoryName.ToLower()) : true)).Id;
                   
                    var itemListPrices = new List<UpdateDrugPriceDto>();
                    UpdateDrugPriceDto? updateDrugPriceDto = null;


                    if (_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MainUnitPrice").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MainUnitPrice").Index))?.ToString().Trim())) &&
                       _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "FullPackPrice").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "FullPackPrice").Index))?.ToString().Trim())) &&
                       _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubUnitPrice").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubUnitPrice").Index))?.ToString().Trim())) &&
                       _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index) != null && (!string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, (DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index))?.ToString().Trim())))
                    {
                        updateDrugPriceDto = new UpdateDrugPriceDto()
                        {
                            Id = (_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3) == null ||
                            string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim())) ? 0
                            : int.Parse(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3)?.ToString()?.Trim()),

                            MainUnitPrice = double.Parse((_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MainUnitPrice").Index))?.ToString().Trim()),

                            FullPackPrice = double.Parse((_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "FullPackPrice").Index))?.ToString().Trim()),
                            SubUnitPrice = double.Parse((_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubUnitPrice").Index))?.ToString().Trim()),
                            EffectiveDateFrom = Convert.ToDateTime((_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index)).ToString().Trim()).Date,
                            EffectiveDateTo = (_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index) == null
                            || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim())) ? null
                            : Convert.ToDateTime(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        };
                        itemListPrices.Add(updateDrugPriceDto);
                    }


                    var updateDrugUHIABasicDataDto = new UpdateDrugUHIABasicDataDto
                    {
                        Id = (_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthDrugCode").Index)?.ToString().Trim(),
                        LocalDrugCode = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "LocalDrugCode").Index)?.ToString().Trim(),
                        InternationalNonProprietaryName = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "InternationalNonProprietaryName").Index)?.ToString().Trim(),
                        ProprietaryName = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ProprietaryName").Index)?.ToString().Trim(),
                        DosageForm = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DosageForm").Index)?.ToString().Trim(),
                        RouteOfAdministration = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "RouteOfAdministration").Index)?.ToString().Trim(),
                        Manufacturer = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Manufacturer").Index)?.ToString().Trim(),
                        MarketAuthorizationHolder = _excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MarketAuthorizationHolder").Index)?.ToString().Trim(),
                        RegistrationTypeId = RegistrationTypeId,
                        DrugsPackageTypeId = drugsPackageTypeId,
                        MainUnitId = mainUnitId,
                        NumberOfMainUnit = int.TryParse(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NumberOfMainUnit").Index)?.ToString().Trim(), out int temp) ? temp : default(int?),
                        SubUnitId = subUnitId,
                        NumberOfSubunitPerMainUnit = int.TryParse(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NumberOfSubunitPerMainUnit").Index)?.ToString().Trim(), out int temp2) ? temp2 : default(int?),
                        TotalNumberSubunitsOfPack = int.TryParse(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "TotalNumberSubunitsOfPack").Index)?.ToString().Trim(), out int temp3) ? temp3 : default(int?),
                        ReimbursementCategoryId = ReimbursementCategoryId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };


                var drugUhiaBulkUploadDto = new DrugUhiaBulkUploadDto
                    {
                        updateDrugUHIABasicDataDto = updateDrugUHIABasicDataDto,
                        updateDrugUHIAPriceDto = new UpdateDrugUHIAPriceDto
                        {
                            drugPrices = itemListPrices,
                            Id = (_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1).ToString()?.Trim()),
                        },
                        RowNumber = row
                    };
                    #endregion
                    if (updateDrugUHIABasicDataDto.Id == Guid.Empty)
                    {
                        var drugsUhia = updateDrugUHIABasicDataDto.ToDrugsUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await drugsUhia.ValidateObjectForBulkUpload(_drugsUHIARepository, _validationEngine);
                        foreach (var item in drugUhiaBulkUploadDto.updateDrugUHIAPriceDto.drugPrices)
                        {
                            var itemListPrice = item.ToDrugPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            var itemErrors = _validationEngine.Validate(itemListPrice, false);
                            if (itemErrors != null)
                            {
                                foreach (var error in itemErrors)
                                {
                                    errors += error.ErrorMessage + "\r\n";
                                }
                            }

                            if (item.EffectiveDateFrom.Date < drugsUhia.DataEffectiveDateFrom.Date ||
                                (item.EffectiveDateTo.HasValue && drugsUhia.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > drugsUhia.DataEffectiveDateTo.Value.Date) ||
                                ((!item.EffectiveDateTo.HasValue) && drugsUhia.DataEffectiveDateTo.HasValue))
                            {
                                errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                            }
                        }

                        drugUhiaBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        drugUhiaBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var drugsUhia = await DrugUHIA.Get(updateDrugUHIABasicDataDto.Id, _drugsUHIARepository);

                        if (drugsUhia is null)
                        {
                            errors += "drugsUhia with DrugsUHIAId not exist." + "\r\n";
                        }
                        else
                        {
                            drugsUhia = updateDrugUHIABasicDataDto.ToDrugsUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await drugsUhia.ValidateObjectForBulkUpload(_drugsUHIARepository, _validationEngine);

                            var notDeletedItems = drugsUhia.DrugPrices.Where(x => x.IsDeleted == false).ToList();
                            foreach (var item in notDeletedItems)
                            {
                                if (item.EffectiveDateFrom.Date < updateDrugUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateDrugUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateDrugUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateDrugUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                    break;
                                }
                            }

                            foreach (var item in drugUhiaBulkUploadDto.updateDrugUHIAPriceDto.drugPrices)
                            {
                                var itemListPrice = item.ToDrugPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                var itemErrors = _validationEngine.Validate(itemListPrice, false);
                                if (itemErrors != null)
                                {
                                    foreach (var error in itemErrors)
                                    {
                                        errors += error.ErrorMessage + "\r\n";
                                    }
                                }

                                if (item.EffectiveDateFrom.Date < updateDrugUHIABasicDataDto.DataEffectiveDateFrom.Date ||
                                    (item.EffectiveDateTo.HasValue && updateDrugUHIABasicDataDto.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > updateDrugUHIABasicDataDto.DataEffectiveDateTo.Value.Date) ||
                                    ((!item.EffectiveDateTo.HasValue) && updateDrugUHIABasicDataDto.DataEffectiveDateTo.HasValue))
                                {
                                    errors += "Price's effective dates must be within the bounds of basic item data effective dates." + "\r\n";
                                }
                                if (item.Id != 0)
                                {
                                    notDeletedItems = notDeletedItems.Where(x => x.Id != item.Id).ToList();
                                }
                                notDeletedItems.Add(item.ToDrugPrice(_identityProvider.GetUserId(), _identityProvider.GetTenantId()));
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
                        drugUhiaBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        drugUhiaBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(drugUhiaBulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthDrugCode").Index, "Duplicated EHealthCode");
                    }

                    if (!(drugUhiaBulkUploadDto.errors.Contains("Duplicated LocalDrugCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "LocalDrugCode").Index, "Duplicated LocalDrugCode");
                    }



                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        drugUhiaBulkUploadDto.IsValid = false;
                        drugUhiaBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!drugUhiaBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue(drugUhiaBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 4);
                        cellRow14.SetCellValue("");
                    }
                    list.Add(drugUhiaBulkUploadDto);

                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.updateDrugUHIABasicDataDto.Id == Guid.Empty)
                        {
                            var drugsWithoutId = item.updateDrugUHIABasicDataDto.ToDrugsUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await drugsWithoutId.Create(_drugsUHIARepository, _validationEngine);

                            var drugsUhia = await DrugUHIA.Get(drugsWithoutId.Id, _drugsUHIARepository);

                            foreach (var price in item.updateDrugUHIAPriceDto.drugPrices)
                            {
                                var itemListPrice = price.ToDrugPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                                drugsUhia.DrugPrices.Add(itemListPrice);
                            }
                            await drugsUhia.Update(_drugsUHIARepository, _validationEngine,userId);
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                            CellId.SetCellValue(drugsUhia?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);
                            var cellpriceId = worksheet.GetRow(item.RowNumber).CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3);
                            cellpriceId.SetCellValue(drugsUhia?.DrugPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()?.ToString());
                            #endregion
                        }
                        else
                        {
                            var drugsUhia = await DrugUHIA.Get(item.updateDrugUHIABasicDataDto.Id, _drugsUHIARepository);
                            drugsUhia.SetEHealthDrugCode(item.updateDrugUHIABasicDataDto.EHealthCode);
                            drugsUhia.SetLocalDrugCode(item.updateDrugUHIABasicDataDto.LocalDrugCode);
                            drugsUhia.SetInternationalNonProprietaryName(item.updateDrugUHIABasicDataDto.InternationalNonProprietaryName);
                            drugsUhia.SetProprietaryName(item.updateDrugUHIABasicDataDto.ProprietaryName);
                            drugsUhia.SetDosageForm(item.updateDrugUHIABasicDataDto.DosageForm);
                            drugsUhia.SetRouteOfAdministration(item.updateDrugUHIABasicDataDto.RouteOfAdministration);
                            drugsUhia.SetManufacturer(item.updateDrugUHIABasicDataDto.Manufacturer);
                            drugsUhia.SetMarketAuthorizationHolder(item.updateDrugUHIABasicDataDto.MarketAuthorizationHolder);
                            drugsUhia.SetRegistrationTypeId(item.updateDrugUHIABasicDataDto.RegistrationTypeId);
                            drugsUhia.SetMarketAuthorizationHolder(item.updateDrugUHIABasicDataDto.MarketAuthorizationHolder);
                            drugsUhia.SetDrugsPackageTypeId(item.updateDrugUHIABasicDataDto.DrugsPackageTypeId);
                            drugsUhia.SetMainUnitId(item.updateDrugUHIABasicDataDto.MainUnitId);
                            drugsUhia.SetNumberOfMainUnit(item.updateDrugUHIABasicDataDto.NumberOfMainUnit);
                            drugsUhia.SetSubUnitId(item.updateDrugUHIABasicDataDto.SubUnitId);
                            drugsUhia.SetNumberOfSubunitPerMainUnit(item.updateDrugUHIABasicDataDto.NumberOfSubunitPerMainUnit);
                            drugsUhia.SetTotalNumberSubunitsOfPack(item.updateDrugUHIABasicDataDto.TotalNumberSubunitsOfPack);
                            drugsUhia.SetReimbursementCategoryId(item.updateDrugUHIABasicDataDto.ReimbursementCategoryId);
                            drugsUhia.SetDataEffectiveDateFrom(item.updateDrugUHIABasicDataDto.DataEffectiveDateFrom);
                            drugsUhia.SetDataEffectiveDateTo(item.updateDrugUHIABasicDataDto.DataEffectiveDateTo);
                            drugsUhia.SetModifiedBy(_identityProvider.GetUserName());
                            drugsUhia.SetModifiedOn();

                            // prepare model to update and soft delete Item Prices
                            for (int i = 0; i < drugsUhia.DrugPrices.Count; i++)
                            {
                                var itemPrice = item.updateDrugUHIAPriceDto.drugPrices.Where(x => x.Id == drugsUhia.DrugPrices[i].Id).FirstOrDefault();
                                if (itemPrice == null)
                                {
                                    continue; // do not apply soft delete now

                                    //serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                                    //serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                                }
                                else
                                {
                                    drugsUhia.DrugPrices[i].SetMainUnitPrice(itemPrice.MainUnitPrice);
                                    drugsUhia.DrugPrices[i].SetFullPackPrice(itemPrice.FullPackPrice);
                                    drugsUhia.DrugPrices[i].SetSubUnitPrice(itemPrice.SubUnitPrice);
                                    drugsUhia.DrugPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                                    drugsUhia.DrugPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                                    drugsUhia.DrugPrices[i].SetModifiedBy(userId);
                                }
                                drugsUhia.DrugPrices[i].SetModifiedOn();
                            }

                            // prepare model to add new Item Prices
                            var addItemPrices = item.updateDrugUHIAPriceDto.drugPrices.Where(x => x.Id == 0).ToList();
                            foreach (var addedItem in addItemPrices)
                            {
                                var itemListPrice = addedItem.ToDrugPrice(userId, tenantId);
                                drugsUhia.DrugPrices.Add(itemListPrice);
                            }

                            await drugsUhia.Update(_drugsUHIARepository, _validationEngine,userId);
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
