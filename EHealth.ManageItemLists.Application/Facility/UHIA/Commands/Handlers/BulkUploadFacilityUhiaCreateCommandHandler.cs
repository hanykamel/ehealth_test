using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands.Handlers
{
    public class BulkUploadFacilityUhiaCreateCommandHandler : IRequestHandler<BulkUploadFacilityCreateCommand, byte[]?>
    {
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IExcelService _excelService;
        public BulkUploadFacilityUhiaCreateCommandHandler(IFacilityUHIARepository facilityUHIARepository,
          IValidationEngine validationEngine,
        IIdentityProvider identityProvider,
        ICategoriesRepository categoriesRepository,
        ISubCategoriesRepository subCategoriesRepository,
        IItemListRepository itemListsRepository,
        IExcelService excelService)
        {
            _facilityUHIARepository = facilityUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _categoriesRepository = categoriesRepository;
            _subCategoriesRepository = subCategoriesRepository;
            _itemListsRepository = itemListsRepository;
            _excelService = excelService;
        }
        public async Task<byte[]?> Handle(BulkUploadFacilityCreateCommand request, CancellationToken cancellationToken)
        {

            _validationEngine.Validate(request);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            var list = new List<FacilityUhiaBulkUploadDto>();
            var validListRows = new List<int>();
            var stream = new MemoryStream();
            await request.file.CopyToAsync(stream);
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);

            int itemListId = int.Parse(_excelService.GetCell(worksheet, 1, (FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 2)).ToString());

            // Throw exception if item list busy
            await FacilityUHIA.IsItemListBusy(_facilityUHIARepository, itemListId);
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
                var cell014 = worksheet.GetRow(0).CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 3);
                cell014.SetCellValue("Errors");
                worksheet.AutoSizeColumn(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 3);
                cell014.CellStyle = headerStyle;
                #endregion
                var rowCounts = worksheet.LastRowNum;
                var categories = await Domain.Categories.Category.Search(_categoriesRepository, f => f.IsDeleted == false, 1, 1, false);
                var subcategories = await Domain.Sub_Categories.SubCategory.Search(_subCategoriesRepository, f => f.IsDeleted == false, 1, 1, false);
               
                for (int row = 1; row <= rowCounts; row++)
                {
                    string errors = "";
                    #region fill objects 
                    var catName = _excelService.GetCell(worksheet, row, (FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index))?.ToString()?.Trim();
                    var catId = categories.Data.FirstOrDefault(c => c.CategoryEn == catName).Id;

                    var subCatName = _excelService.GetCell(worksheet, row, (FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index))?.ToString()?.Trim();
                    var subCatId = subcategories.Data.FirstOrDefault(s => s.SubCategoryEn == subCatName).Id;



                    var updateFacilityUHIADto = new UpdateFacilityUHIADto
                    {
                        Id = (_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 1) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 1)?.ToString()?.Trim())) ? Guid.Empty : new Guid(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 1)?.ToString()?.Trim()),
                        EHealthCode = _excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index)?.ToString().Trim(),
                        DescriptorAr = _excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index)?.ToString().Trim(),
                        DescriptorEn = _excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index)?.ToString().Trim(),
                        OccupancyRate = double.TryParse(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "OccupancyRate").Index)?.ToString().Trim(), out double temp) ? temp : default(int?),
                        OperatingRateInHoursPerDay = double.Parse(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "OperatingRateInHoursPerDay").Index)?.ToString().Trim()),
                        OperatingDaysPerMonth = double.Parse(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "OperatingDaysPerMonth").Index)?.ToString().Trim()),
                        CategoryId = catId,
                        SubCategoryId = subCatId,
                        DataEffectiveDateFrom = Convert.ToDateTime(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index)?.ToString().Trim()).Date,
                        DataEffectiveDateTo = (_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index) == null || string.IsNullOrEmpty(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim())) ? null : Convert.ToDateTime(_excelService.GetCell(worksheet, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index)?.ToString()?.Trim()).Date,
                        ItemListId = itemListId,
                    };
                

                    var facilityUhiaBulkUploadDto = new FacilityUhiaBulkUploadDto
                    {
                        updateFacilityUHIADto = updateFacilityUHIADto,
                        RowNumber = row
                    };
                    #endregion
                    if (updateFacilityUHIADto.Id == Guid.Empty)
                    {
                        var facilityUhia = updateFacilityUHIADto.ToFacilityUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                        errors = await facilityUhia.ValidateObjectForBulkUpload(_facilityUHIARepository, _validationEngine);


                        facilityUhiaBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        facilityUhiaBulkUploadDto.errors = errors;

                    }
                    else
                    {
                        var facilityUhia = await FacilityUHIA.Get(updateFacilityUHIADto.Id, _facilityUHIARepository);

                        if (facilityUhia is null)
                        {
                            errors += "facilityUhia with facilityUhiaId not exist." + "\r\n";
                        }
                        else
                        {
                            facilityUhia = updateFacilityUHIADto.ToFacilityUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

                            errors = await facilityUhia.ValidateObjectForBulkUpload(_facilityUHIARepository, _validationEngine);


                        }
                        facilityUhiaBulkUploadDto.IsValid = string.IsNullOrEmpty(errors) ? true : false;
                        facilityUhiaBulkUploadDto.errors = errors;

                    }

                    #region // Check duplication in file rows
                    string dublicatedProperties = "";
                    if (!(facilityUhiaBulkUploadDto.errors.Contains("Duplicated EHealthCode")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index, "Duplicated EHealthCode");
                    }

                    if (!(facilityUhiaBulkUploadDto.errors.Contains("Duplicated ShortDescAr")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index, "Duplicated ShortDescAr");
                    }

                    if (!(facilityUhiaBulkUploadDto.errors.Contains("Duplicated ShortDescEn")))
                    {

                        _excelService.CheckDupllicatesInFile(worksheet, rowCounts, ref dublicatedProperties, row, FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index, "Duplicated ShortDescEn");
                    }


                    if (!string.IsNullOrEmpty(dublicatedProperties))
                    {
                        facilityUhiaBulkUploadDto.IsValid = false;
                        facilityUhiaBulkUploadDto.errors += dublicatedProperties;
                    }
                    #endregion

                    if (!facilityUhiaBulkUploadDto.IsValid)
                    {

                        var cellRow14 = worksheet.GetRow(row).CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 3);
                        cellRow14.SetCellValue(facilityUhiaBulkUploadDto.errors);
                        worksheet.AutoSizeColumn(cellRow14.ColumnIndex);
                    }
                    else
                    {
                        var cellRow14 = worksheet.GetRow(row).CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 3);
                        cellRow14.SetCellValue("");
                    }
                    list.Add(facilityUhiaBulkUploadDto);
                }



                foreach (var item in list)
                {
                    if (item.IsValid == true)
                    {
                        if (item.updateFacilityUHIADto.Id == Guid.Empty)
                        {
                            var facilityWithoutId = item.updateFacilityUHIADto.ToFacilityUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                            await facilityWithoutId.Create(_facilityUHIARepository, _validationEngine);

                            var facilityUhia = await FacilityUHIA.Get(facilityWithoutId.Id, _facilityUHIARepository);


                            await facilityUhia.Update(_facilityUHIARepository, _validationEngine,userId);
                            #region update Cells of added row (id)
                            var CellId = worksheet.GetRow(item.RowNumber).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 1);
                            CellId.SetCellValue(facilityUhia?.Id.ToString());
                            var cellItemList = worksheet.GetRow(item.RowNumber).CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 2);
                            cellItemList.SetCellValue(itemListId);

                            #endregion
                        }
                        else
                        {
                            var facilityUhia =await FacilityUHIA.Get(item.updateFacilityUHIADto.Id, _facilityUHIARepository);
                            
                            facilityUhia.SetCode(item.updateFacilityUHIADto.EHealthCode);
                            facilityUhia.SetDescriptorAr(item.updateFacilityUHIADto.DescriptorAr);
                            facilityUhia.SetDescriptorEn(item.updateFacilityUHIADto.DescriptorEn);
                            facilityUhia.SetCategoryId(item.updateFacilityUHIADto.CategoryId);
                            facilityUhia.SetSubCategoryId(item.updateFacilityUHIADto.SubCategoryId);
                            facilityUhia.SetDataEffectiveDateFrom(item.updateFacilityUHIADto.DataEffectiveDateFrom);
                            facilityUhia.SetDataEffectiveDateTo(item.updateFacilityUHIADto.DataEffectiveDateTo);
                            facilityUhia.SetModifiedBy(_identityProvider.GetUserName());
                            facilityUhia.SetModifiedOn();



                            await facilityUhia.Update(_facilityUHIARepository, _validationEngine,userId);
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
