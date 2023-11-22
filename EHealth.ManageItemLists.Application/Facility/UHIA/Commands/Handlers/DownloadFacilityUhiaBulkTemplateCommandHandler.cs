using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries;
using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands;
using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Drugs.UHIA.Queries;
using EHealth.ManageItemLists.Application.Excel;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Facility.UHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands.Handlers
{
    public class DownloadFacilityUhiaBulkTemplateCommandHandler : IRequestHandler<DownloadFacilityUhiaBulkTemplateCommand, byte[]>
    {
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IMediator _mediator;
        public DownloadFacilityUhiaBulkTemplateCommandHandler(IFacilityUHIARepository facilityUHIARepository,IMediator mediator)
        {
            _facilityUHIARepository = facilityUHIARepository;
            _mediator = mediator;   
        }
        public async Task<byte[]> Handle(DownloadFacilityUhiaBulkTemplateCommand request, CancellationToken cancellationToken)
        {
            var facilityUHIASearchQuery = new FacilityUHIASearchQuery();

            facilityUHIASearchQuery.ItemListId = request.ItemListId;
            facilityUHIASearchQuery.Code = request.Code;
            facilityUHIASearchQuery.DescriptorAr = request.DescriptorAr;
            facilityUHIASearchQuery.DescriptorEn = request.DescriptorEn;
            facilityUHIASearchQuery.OrderBy = request.OrderBy;
            facilityUHIASearchQuery.Ascending = request.Ascending;

            var res = await _mediator.Send(facilityUHIASearchQuery);
            res.Data = res.Data.Where(s => s.IsDeleted != true).ToList();
            //generate excel template 
            MemoryStream output = new MemoryStream();
            ExportToExcel<FacilityUHIADto> exportClass = new ExportToExcel<FacilityUHIADto>(_mediator)
            {
                Data = (List<FacilityUHIADto>)(res?.Data),
                Columns = FacilityUhiaTemplateHeader.Headers
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
     
                var cell1 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index);
                cell1.SetCellValue(res.Data[i].DescriptorAr);

                var cell2 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index);
                cell2.SetCellValue(res.Data[i].DescriptorEn);

                var cell3 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "OccupancyRate").Index);
                cell3.SetCellValue(res.Data[i]?.OccupancyRate?.ToString());

                var cell4 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "OperatingRateInHoursPerDay").Index);
                cell4.SetCellValue(res.Data[i]?.OperatingRateInHoursPerDay.ToString());

                var cell5 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "OperatingDaysPerMonth").Index);
                cell5.SetCellValue(res.Data[i]?.OperatingDaysPerMonth.ToString());

                var cell6 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index);
                cell6.SetCellValue(res.Data[i]?.Category?.CategoryEn);

                var cell7 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index);
                cell7.SetCellValue(res.Data[i]?.SubCategory?.SubCategoryEn);

                var cell8 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index);
                cell8.SetCellValue(res.Data[i]?.DataEffectiveDateFrom.ToString());

                var cell9 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index);
                cell9.SetCellValue(res.Data[i]?.DataEffectiveDateTo?.ToString());



                var cell10 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 1);
                cell10.SetCellValue(res.Data[i].Id.ToString());
                var cell11 = row.CreateCell(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 2);
                cell11.SetCellValue(res.Data[i].ItemListId.ToString());

            }
            //hide id and item list id columns
            basicSheet.SetColumnHidden(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 1, true);
            basicSheet.SetColumnHidden(FacilityUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index + 2, true);


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
            var EHealthCodeIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EHealthCode").Index;
            CellRangeAddressList EHealthCodecell = new CellRangeAddressList(1, 999999, EHealthCodeIndex, EHealthCodeIndex);
            IDataValidationConstraint EHealthCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "500");
            IDataValidation EHealthCodeLengthValidation = validationHelper.CreateValidation(EHealthCodeValidation, EHealthCodecell);
            EHealthCodeLengthValidation.ShowErrorBox = true;
            EHealthCodeLengthValidation.CreateErrorBox("", "Please Insert EHealthCode data from 1 to 50 characters");
            EHealthCodeLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(EHealthCodeLengthValidation);
            #endregion


            #region ShortDescAr validation
            var ShortDescArIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DescriptorAr").Index;
            CellRangeAddressList ShortDescArcell = new CellRangeAddressList(1, 999999, ShortDescArIndex, ShortDescArIndex);
            IDataValidationConstraint ShortDescArValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "100");
            IDataValidation ShortDescArLengthValidation = validationHelper.CreateValidation(ShortDescArValidation, ShortDescArcell);
            ShortDescArLengthValidation.ShowErrorBox = true;
            ShortDescArLengthValidation.CreateErrorBox("", "Please Insert DescriptorAr data from 1 to 100 characters");
            ShortDescArLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ShortDescArLengthValidation);
            #endregion

            #region ShortDescEn validation
            var ShortDescEnIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DescriptorEn").Index;
            CellRangeAddressList ShortDescEncell = new CellRangeAddressList(1, 999999, ShortDescEnIndex, ShortDescEnIndex);
            IDataValidationConstraint ShortDescEnValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "100");
            IDataValidation ShortDescEnLengthValidation = validationHelper.CreateValidation(ShortDescEnValidation, ShortDescEncell);
            ShortDescEnLengthValidation.ShowErrorBox = true;
            ShortDescEnLengthValidation.CreateErrorBox("", "Please Insert DescriptorEn data from 1 to 100 characters");
            ShortDescEnLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(ShortDescEnLengthValidation);
            #endregion

            #region OccupancyRate validation
            var OccupancyRateIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "OccupancyRate").Index;
            CellRangeAddressList OccupancyRatecell = new CellRangeAddressList(1, 999999, OccupancyRateIndex, OccupancyRateIndex);
            IDataValidationConstraint OccupancyRateValidation = validationHelper.CreateintConstraint(OperatorType.BETWEEN, "10", "99");
            IDataValidation OccupancyRateLengthValidation = validationHelper.CreateValidation(OccupancyRateValidation, OccupancyRatecell);
            OccupancyRateLengthValidation.ShowErrorBox = true;
            OccupancyRateLengthValidation.CreateErrorBox("", "Please Insert OccupancyRate data from 10 to 99 characters");
            OccupancyRateLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(OccupancyRateLengthValidation);
            #endregion

            #region OperatingRateInHoursPerDay validation
            var OperatingRateInHoursPerDayIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "OperatingRateInHoursPerDay").Index;
            CellRangeAddressList OperatingRateInHoursPerDaycell = new CellRangeAddressList(1, 999999, OperatingRateInHoursPerDayIndex, OperatingRateInHoursPerDayIndex);
            IDataValidationConstraint OperatingRateInHoursPerDayValidation = validationHelper.CreateintConstraint(OperatorType.BETWEEN, "10", "99");
            IDataValidation OperatingRateInHoursPerDayLengthValidation = validationHelper.CreateValidation(OperatingRateInHoursPerDayValidation, OperatingRateInHoursPerDaycell);
            OperatingRateInHoursPerDayLengthValidation.ShowErrorBox = true;
            OperatingRateInHoursPerDayLengthValidation.CreateErrorBox("", "Please Insert OperatingRateInHoursPerDay data from 10 to 99 characters");
            OperatingRateInHoursPerDayLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(OperatingRateInHoursPerDayLengthValidation);
            #endregion

            #region OperatingDaysPerMonth validation
            var OperatingDaysPerMonthIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "OperatingDaysPerMonth").Index;
            CellRangeAddressList OperatingDaysPerMonthcell = new CellRangeAddressList(1, 999999, OperatingDaysPerMonthIndex, OperatingDaysPerMonthIndex);
            IDataValidationConstraint OperatingDaysPerMonthValidation = validationHelper.CreateintConstraint(OperatorType.BETWEEN, "10", "99");
            IDataValidation OperatingDaysPerMonthLengthValidation = validationHelper.CreateValidation(OperatingDaysPerMonthValidation, OperatingDaysPerMonthcell);
            OperatingDaysPerMonthLengthValidation.ShowErrorBox = true;
            OperatingDaysPerMonthLengthValidation.CreateErrorBox("", "Please Insert OperatingDaysPerMonth data from 10 to 99 characters");
            OperatingDaysPerMonthLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(OperatingDaysPerMonthLengthValidation);
            #endregion

            #region DataEffectiveDateFrom validation
            var FromDateIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateFrom").Index;
            CellRangeAddressList FromDateAddress = new CellRangeAddressList(1, 999999, FromDateIndex, FromDateIndex);
            IDataValidationConstraint FromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation FromDateValidation = validationHelper.CreateValidation(FromDateDate, FromDateAddress);
            FromDateValidation.ShowErrorBox = true;
            FromDateValidation.CreateErrorBox("wrong DataEffectiveDateFrom", "please enter right DataEffectiveDateFrom (yyyy-mm-dd)");
            FromDateValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(FromDateValidation);
            #endregion

      

            #region DataEffectiveDateTo validation
            var ToDateIndex = FacilityUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateTo").Index;
            CellRangeAddressList ToDateAddress = new CellRangeAddressList(1, 999999, ToDateIndex, ToDateIndex);
            IDataValidationConstraint ToDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation ToDateValidation = validationHelper.CreateValidation(ToDateDate, ToDateAddress);
            ToDateValidation.ShowErrorBox = true;
            ToDateValidation.CreateErrorBox("wrong DataEffectiveDateTo", "please enter right DataEffectiveDateTo (yyyy-mm-dd)");
            ToDateValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ToDateValidation);
            #endregion


 

        }
    }
}
