using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EHealth.ManageItemLists.Application.Excel;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.Queries;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using IndexedColors = NPOI.SS.UserModel.IndexedColors;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Handlers
{
    public class DownloadBulkTemplateCommandHandler : IRequestHandler<DownloadBulkTemplateCommand, byte[]>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IMediator _mediator;
        public DownloadBulkTemplateCommandHandler(IServiceUHIARepository serviceUHIARepository, IMediator mediator)
        {
            _mediator = mediator;
            _serviceUHIARepository = serviceUHIARepository;
        }


        public async Task<byte[]> Handle(DownloadBulkTemplateCommand request, CancellationToken cancellationToken)
        {
            //get data
            var serviceUHIASearchQuery = new ServiceUHIASearchQuery();

            serviceUHIASearchQuery.ItemListId = request.ItemListId;
            serviceUHIASearchQuery.EHealthCode = request.EHealthCode;
            serviceUHIASearchQuery.UHIAId = request.UHIAId;
            serviceUHIASearchQuery.ShortDescriptionAr = request.ShortDescriptionAr;
            serviceUHIASearchQuery.ShortDescriptionEn = request.ShortDescriptionEn;

            var res = await _mediator.Send(serviceUHIASearchQuery);
            res.Data = res.Data.Where(s => s.IsDeleted != true).ToList();
            //generate excel template 
            MemoryStream output = new MemoryStream();
            ExportToExcel<ServiceUHIADto> exportClass = new ExportToExcel<ServiceUHIADto>(_mediator)
            {
                Data = (List<ServiceUHIADto>)(res?.Data),
                Columns = ServiceUhiaTemplateHeader.Headers
            };
            var workbook = await exportClass.GenerateTemplate(request.ItemListSubtypeId);
            var basicSheet = (XSSFSheet)workbook.GetSheetAt(0);
            AddValidation(basicSheet);
            //var basicSheet = workbook.GetSheetAt(0);
            if (res.Data.Count() > 0)
            {
                for (int i = 0; i < res.Data.Count(); i++)
                {
                    var row = basicSheet.CreateRow(i + 1);
                    var cell0 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EHealthCode").Index);

                    cell0.SetCellValue(res.Data[i].EHealthCode);
                    var cell1 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "UHIAId").Index);
                    cell1.SetCellValue(res.Data[i].UHIAId);
                    var cell2 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "ShortDescAr").Index);
                    cell2.SetCellValue(res.Data[i].ShortDescAr);
                    var cell3 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "ShortDescEn").Index);
                    cell3.SetCellValue(res.Data[i].ShortDescEn);
                    var cell4 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "Category").Index);
                    cell4.SetCellValue(res.Data[i]?.ServiceCategory?.CategoryEn);
                    var cell5 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "SubCategory").Index);
                    cell5.SetCellValue(res.Data[i]?.ServiceSubCategory?.SubCategoryEn);
                    var cell6 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateFrom").Index);
                    cell6.SetCellValue(res.Data[i].DataEffectiveDateFrom.ToString());
                    var cell7 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateTo").Index);
                    cell7.SetCellValue(res.Data[i]?.DataEffectiveDateTo?.ToString());
                    var cell8 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "Price").Index);
                    cell8.SetCellValue(res.Data[i]?.ItemListPrice?.Price.ToString());
                    var cell9 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateFrom").Index);
                    cell9.SetCellValue(res.Data[i]?.ItemListPrice?.EffectiveDateFrom.ToString());
                    var cell10 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index);
                    cell10.SetCellValue(res.Data[i]?.ItemListPrice?.EffectiveDateTo?.ToString());
                    var cell11 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index+1);
                    cell11.SetCellValue(res.Data[i].Id.ToString());
                    var cell12 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index+2);
                    cell12.SetCellValue(res.Data[i].ItemListId);
                    var cell13 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index+3);
                    cell13.SetCellValue(res.Data[i]?.ItemListPrice?.Id.ToString());
                }
            }
            else
            {
                var row = basicSheet.CreateRow(1);
                var cell0 = row.CreateCell(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                cell0.SetCellValue(request.ItemListId);
            }

            //hide id column to know which item updated or added
            basicSheet.SetColumnHidden(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1, true);
            basicSheet.SetColumnHidden(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2, true);
            basicSheet.SetColumnHidden(ServiceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3, true);

            //adjust column width
            var header = basicSheet.GetRow(0);
            for (int i = 0; i < header.Cells.Count; i++)
            {
                basicSheet.AutoSizeColumn(i);
            }
            workbook.Write(output);
            return output.ToArray();

        }
        public void AddValidation(XSSFSheet basicSheet) {
            IDataValidationHelper validationHelper = new XSSFDataValidationHelper(basicSheet);
            #region EHealthCode validation
            var EHealthCodeIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EHealthCode").Index;
            CellRangeAddressList EHealthCodecell = new CellRangeAddressList(1, 999999, EHealthCodeIndex, EHealthCodeIndex);
            IDataValidationConstraint EHealthCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN,"1", "500");
            IDataValidation EHealthCodeLengthValidation = validationHelper.CreateValidation(EHealthCodeValidation, EHealthCodecell);
            EHealthCodeLengthValidation.ShowErrorBox = true;
            EHealthCodeLengthValidation.CreateErrorBox("","Please Insert data from 1 to 500 characters");
            EHealthCodeLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(EHealthCodeLengthValidation);
            #endregion

            #region UHIAId validation
            var UHIAIdIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "UHIAId").Index;
            CellRangeAddressList UHIAIdcell = new CellRangeAddressList(1, 999999, UHIAIdIndex, UHIAIdIndex);
            IDataValidationConstraint optionsValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "500");
            IDataValidation UHIAIdLengthValidation = validationHelper.CreateValidation(optionsValidation, UHIAIdcell);
            UHIAIdLengthValidation.ShowErrorBox = true;
            UHIAIdLengthValidation.CreateErrorBox("", "Please Insert data from 1 to 500 characters");
            UHIAIdLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(UHIAIdLengthValidation);
            #endregion

            #region UHIAId validation
            var ShortDescArIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "ShortDescAr").Index;
            CellRangeAddressList ShortDescArcell = new CellRangeAddressList(1, 999999, ShortDescArIndex, ShortDescArIndex);
            IDataValidationConstraint ShortDescArValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "4", "60");
            IDataValidation ShortDescArLengthValidation = validationHelper.CreateValidation(ShortDescArValidation, ShortDescArcell);
            ShortDescArLengthValidation.ShowErrorBox = true;
            ShortDescArLengthValidation.CreateErrorBox("", "Please Insert data from 4 to 60 characters");
            ShortDescArLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ShortDescArLengthValidation);
            #endregion

            #region UHIAId validation
            var ShortDescEnIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "ShortDescEn").Index;
            CellRangeAddressList ShortDescEncell = new CellRangeAddressList(1, 999999, ShortDescEnIndex, ShortDescEnIndex);
            IDataValidationConstraint ShortDescEnValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "4", "60");
            IDataValidation ShortDescEnLengthValidation = validationHelper.CreateValidation(ShortDescEnValidation, ShortDescEncell);
            ShortDescEnLengthValidation.ShowErrorBox = true;
            ShortDescEnLengthValidation.CreateErrorBox("", "Please Insert data from 4 to 60 characters");
            ShortDescEnLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(ShortDescEnLengthValidation);
            #endregion


            #region DataEffectiveDateFrom validation
            var FromDateIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateFrom").Index;
            CellRangeAddressList FromDateAddress = new CellRangeAddressList(1, 999999, FromDateIndex, FromDateIndex);
            IDataValidationConstraint FromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)","yyyy-mm-dd");
            IDataValidation FromDateValidation = validationHelper.CreateValidation(FromDateDate, FromDateAddress);
            FromDateValidation.ShowErrorBox = true;
            FromDateValidation.CreateErrorBox("wrong DataEffectiveDateFrom", "please enter right DataEffectiveDateFrom (yyyy-mm-dd)");
            FromDateValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(FromDateValidation);
            #endregion

            #region DataEffectiveDateTo validation
            var ToDateIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateTo").Index;
            CellRangeAddressList ToDateAddress = new CellRangeAddressList(1, 999999, ToDateIndex, ToDateIndex);
            IDataValidationConstraint ToDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation ToDateValidation = validationHelper.CreateValidation(ToDateDate, ToDateAddress);
            ToDateValidation.ShowErrorBox = true;
            ToDateValidation.CreateErrorBox("wrong DataEffectiveDateTo", "please enter right DataEffectiveDateTo (yyyy-mm-dd)");
            ToDateValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ToDateValidation);
            #endregion

            #region PriceDataEffectiveDateFrom validation
            var PriceFromDateIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateFrom").Index;
            CellRangeAddressList PriceFromDateAddress = new CellRangeAddressList(1, 999999, PriceFromDateIndex, PriceFromDateIndex);
            IDataValidationConstraint PriceFromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation PriceFromDateValidation = validationHelper.CreateValidation(PriceFromDateDate, PriceFromDateAddress);
            FromDateValidation.ShowErrorBox = true;
            FromDateValidation.CreateErrorBox("wrong Price EffectiveDateFrom", "please enter right Price EffectiveDateFrom (yyyy-mm-dd)");
            FromDateValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(PriceFromDateValidation);
            #endregion

            #region PriceDataEffectiveDateTo validation
            var PriceToDateIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index;
            CellRangeAddressList PriceToDateAddress = new CellRangeAddressList(1, 999999, PriceToDateIndex, PriceToDateIndex);
            IDataValidationConstraint PriceToDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation PriceToDateValidation = validationHelper.CreateValidation(PriceToDateDate, PriceToDateAddress);
            PriceToDateValidation.ShowErrorBox = true;
            PriceToDateValidation.CreateErrorBox("wrong Price EffectiveDateTo", "please enter right Price EffectiveDateTo (yyyy-mm-dd)");
            PriceToDateValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(PriceToDateValidation);
            #endregion
            //#region Price validation
            //var PriceIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "Price").Index;
            //CellRangeAddressList Pricecell = new CellRangeAddressList(1, 999999, UHIAIdIndex, UHIAIdIndex);
            //IDataValidationConstraint PriceValidation = validationHelper.CreateDecimalConstraint(OperatorType.BETWEEN, "1.00", "501");
            //IDataValidation PriceLengthValidation = validationHelper.CreateValidation(optionsValidation, UHIAIdcell);
            //PriceLengthValidation.ShowErrorBox = true;
            //PriceLengthValidation.CreateErrorBox("", "Please Insert data between 1 and 501");
            //PriceLengthValidation.EmptyCellAllowed = true;
            //basicSheet.AddValidationData(PriceLengthValidation);
            //#endregion

        }
    }
}
