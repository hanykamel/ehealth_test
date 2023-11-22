using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries;
using EHealth.ManageItemLists.Application.Excel;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Handlers
{
    public class DownloadConsumablesAndDevicesBulkTemplateCommandHandler : IRequestHandler<DownloadConsumablesAndDevicesBulkTemplateCommand, byte[]>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IMediator _mediator;
        public DownloadConsumablesAndDevicesBulkTemplateCommandHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository, IMediator mediator)
        {
            _mediator = mediator;
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
        }
        public async Task<byte[]> Handle(DownloadConsumablesAndDevicesBulkTemplateCommand request, CancellationToken cancellationToken)
        {
            var consAndDevUHIASearchQuery = new ConsAndDevUHIASearchQuery();

            consAndDevUHIASearchQuery.ItemListId = request.ItemListId;
            consAndDevUHIASearchQuery.EHealthCode = request.EHealthCode;
            consAndDevUHIASearchQuery.UHIAId = request.UHIAId;
            consAndDevUHIASearchQuery.ShortDescriptionAr = request.ShortDescriptionAr;
            consAndDevUHIASearchQuery.ShortDescriptionEn = request.ShortDescriptionEn;
            consAndDevUHIASearchQuery.OrderBy = request.OrderBy;
            consAndDevUHIASearchQuery.Ascending = request.Ascending;

            var res = await _mediator.Send(consAndDevUHIASearchQuery);
            res.Data = res.Data.Where(s => s.IsDeleted == false).ToList();
            //generate excel template 
            MemoryStream output = new MemoryStream();
            ExportToExcel<ConsAndDevDto> exportClass = new ExportToExcel<ConsAndDevDto>(_mediator)
            {
                Data = (List<ConsAndDevDto>)(res?.Data),
                Columns = ConsAndDevsUhiaTemplateHeader.Headers
            };
            var workbook = await exportClass.GenerateTemplate(request.ItemListSubtypeId);
            var basicSheet = (XSSFSheet)workbook.GetSheetAt(0);
            AddValidation(basicSheet);
            //fill data

            for (int i = 0; i < res.Data.Count(); i++)
            {
                var row = basicSheet.CreateRow(i + 1);
                var cell0 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index);
                cell0.SetCellValue(res.Data[i].EHealthCode);
                var cell1 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UHIAId").Index);
                cell1.SetCellValue(res.Data[i].UHIAId);
                var cell2 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescriptionAr").Index);
                cell2.SetCellValue(res.Data[i].ShortDescriptionAr);
                var cell3 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ShortDescriptionEn").Index);
                cell3.SetCellValue(res.Data[i].ShortDescriptionEn);
                var cell4 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "LocalUnitOfMeasure").Index);
                cell4.SetCellValue(res.Data[i]?.LocalUnitOfMeasure?.MeasureTypeEn);
                var cell5 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index);
                cell5.SetCellValue(res.Data[i]?.Category?.CategoryEn);
                var cell6 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index);
                cell6.SetCellValue(res.Data[i]?.SubCategory?.SubCategoryEn);
                var cell7 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index);
                cell7.SetCellValue(res.Data[i]?.DataEffectiveDateFrom.ToString());
                var cell8 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index);
                cell8.SetCellValue(res.Data[i]?.DataEffectiveDateTo?.ToString());
                var cell9 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index);
                cell9.SetCellValue(res.Data[i]?.ItemListPrice?.Price.ToString());
                var cell10 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index);
                cell10.SetCellValue(res.Data[i]?.ItemListPrice?.EffectiveDateFrom.ToString());
                var cell11 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index);
                cell11.SetCellValue(res.Data[i]?.ItemListPrice?.EffectiveDateTo?.ToString());
                var cell12 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                cell12.SetCellValue(res.Data[i].Id.ToString());
                var cell13 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                cell13.SetCellValue(res.Data[i].ItemListId.ToString());
                var cell14 = row.CreateCell(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index + 3);
                cell14.SetCellValue(res.Data[i]?.ItemListPrice?.Id.ToString());
            }
            //hide id and item list id columns
            basicSheet.SetColumnHidden(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1, true);
            basicSheet.SetColumnHidden(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2, true);
            basicSheet.SetColumnHidden(ConsAndDevsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3, true);

            //adjust column width
            var header = basicSheet.GetRow(0);
            for (int i = 0; i < header.Cells.Count; i++)
            {
                basicSheet.AutoSizeColumn(i);
            }

            workbook.Write(output);
            return output.ToArray();
        }

        public void AddValidation(XSSFSheet basicSheet)
        {
            IDataValidationHelper validationHelper = new XSSFDataValidationHelper(basicSheet);
            #region EHealthCode validation
            var EHealthCodeIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EHealthCode").Index;
            CellRangeAddressList EHealthCodecell = new CellRangeAddressList(1, 999999, EHealthCodeIndex, EHealthCodeIndex);
            IDataValidationConstraint EHealthCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "100");
            IDataValidation EHealthCodeLengthValidation = validationHelper.CreateValidation(EHealthCodeValidation, EHealthCodecell);
            EHealthCodeLengthValidation.ShowErrorBox = true;
            EHealthCodeLengthValidation.CreateErrorBox("", "Please Insert data from 1 to 100 characters");
            EHealthCodeLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(EHealthCodeLengthValidation);
            #endregion

            #region UHIAId validation
            var UHIAIdIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "UHIAId").Index;
            CellRangeAddressList UHIAIdcell = new CellRangeAddressList(1, 999999, UHIAIdIndex, UHIAIdIndex);
            IDataValidationConstraint optionsValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "501");
            IDataValidation UHIAIdLengthValidation = validationHelper.CreateValidation(optionsValidation, UHIAIdcell);
            UHIAIdLengthValidation.ShowErrorBox = true;
            UHIAIdLengthValidation.CreateErrorBox("", "Please Insert data from 1 to 501 characters");
            UHIAIdLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(UHIAIdLengthValidation);
            #endregion

            #region ShortDescAr validation
            var ShortDescArIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "ShortDescAr").Index;
            CellRangeAddressList ShortDescArcell = new CellRangeAddressList(1, 999999, ShortDescArIndex, ShortDescArIndex);
            IDataValidationConstraint ShortDescArValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "5", "200");
            IDataValidation ShortDescArLengthValidation = validationHelper.CreateValidation(ShortDescArValidation, ShortDescArcell);
            ShortDescArLengthValidation.ShowErrorBox = true;
            ShortDescArLengthValidation.CreateErrorBox("", "Please Insert data from 5 to 200 characters");
            ShortDescArLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ShortDescArLengthValidation);
            #endregion

            #region ShortDescEn validation
            var ShortDescEnIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "ShortDescEn").Index;
            CellRangeAddressList ShortDescEncell = new CellRangeAddressList(1, 999999, ShortDescEnIndex, ShortDescEnIndex);
            IDataValidationConstraint ShortDescEnValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "5", "200");
            IDataValidation ShortDescEnLengthValidation = validationHelper.CreateValidation(ShortDescEnValidation, ShortDescEncell);
            ShortDescEnLengthValidation.ShowErrorBox = true;
            ShortDescEnLengthValidation.CreateErrorBox("", "Please Insert data from 5 to 200 characters");
            ShortDescEnLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ShortDescEnLengthValidation);
            #endregion

            #region DataEffectiveDateFrom validation
            var FromDateIndex = ServiceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateFrom").Index;
            CellRangeAddressList FromDateAddress = new CellRangeAddressList(1, 999999, FromDateIndex, FromDateIndex);
            IDataValidationConstraint FromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
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
