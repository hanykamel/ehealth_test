using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries;
using EHealth.ManageItemLists.Application.Excel;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Handler
{
    public class DownloadDoctorFeesUhiaBulkTemplateCommandHandler : IRequestHandler<DownloadDoctorFeesUhiaBulkTemplateCommand, byte[]>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IMediator _mediator;
        public DownloadDoctorFeesUhiaBulkTemplateCommandHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository, IMediator mediator)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            _mediator = mediator;   

        }

        public async Task<byte[]> Handle(DownloadDoctorFeesUhiaBulkTemplateCommand request, CancellationToken cancellationToken)
        {
            var doctorFeesUHIASearchQuery = new DoctorFeesUHIASearchQuery();

            doctorFeesUHIASearchQuery.ItemListId = request.ItemListId;
            doctorFeesUHIASearchQuery.EHealthCode = request.EHealthCode;
            doctorFeesUHIASearchQuery.DescriptorAr = request.DescriptorAr;
            doctorFeesUHIASearchQuery.DescriptorEn = request.DescriptorEn;
            doctorFeesUHIASearchQuery.OrderBy = request.OrderBy;
            doctorFeesUHIASearchQuery.Ascending = request.Ascending;

            var res = await _mediator.Send(doctorFeesUHIASearchQuery);
            res.Data = res.Data.Where(s => s.IsDeleted != true).ToList();
            //generate excel template 
            MemoryStream output = new MemoryStream();
            ExportToExcel<DoctorFeesUHIADto> exportClass = new ExportToExcel<DoctorFeesUHIADto>(_mediator)
            {
                Data = (List<DoctorFeesUHIADto>)(res?.Data),
                Columns = DoctorFeesUhiaTemplateHeader.Headers
            };
            var workbook = await exportClass.GenerateTemplate(request.itemListSubtypeId);
            var basicSheet = (XSSFSheet)workbook.GetSheetAt(0);
            AddValidation(basicSheet);
            //fill data

            for (int i = 0; i < res.Data.Count(); i++)
            {
                var row = basicSheet.CreateRow(i + 1);
                var cell0 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index);
                cell0.SetCellValue(res.Data[i].EHealthCode);
                var cell1 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index);
                cell1.SetCellValue(res.Data[i].DescriptorAr);
                var cell2 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index);
                cell2.SetCellValue(res.Data[i].DescriptorEn);
                var cell3 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "PackageComplexityClassification").Index);
                cell3.SetCellValue(res.Data[i]?.PackageComplexityClassification?.ComplexityEn);
                var cell4 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index);
                cell4.SetCellValue(res.Data[i]?.DataEffectiveDateFrom.ToString());
                var cell5 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index);
                cell5.SetCellValue(res.Data[i]?.DataEffectiveDateTo?.ToString());
                var cell6 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DoctorFees").Index);
                cell6.SetCellValue(res.Data[i]?.ItemListPrices?.DoctorFees.ToString());
                var cell7 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "UnitOfDoctorFees").Index);
                cell7.SetCellValue(res.Data[i]?.ItemListPrices?.UnitOfDoctorFees?.NameEN);
                var cell8 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index);
                cell8.SetCellValue(res.Data[i]?.ItemListPrices?.EffectiveDateFrom.ToString());
                var cell9 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index);
                cell9.SetCellValue(res.Data[i]?.ItemListPrices?.EffectiveDateTo?.ToString());
                var cell10 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                cell10.SetCellValue(res.Data[i].Id.ToString());
                var cell11 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                cell11.SetCellValue(res.Data[i].ItemListId.ToString());
                var cell12 = row.CreateCell(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index + 3);
                cell12.SetCellValue(res.Data[i]?.ItemListPrices?.Id.ToString());
            }
            //hide id and item list id columns
            basicSheet.SetColumnHidden(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1, true);
            basicSheet.SetColumnHidden(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2, true);
            basicSheet.SetColumnHidden(DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3, true);

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
            var EHealthCodeIndex = DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EHealthCode").Index;
            CellRangeAddressList EHealthCodecell = new CellRangeAddressList(1, 999999, EHealthCodeIndex, EHealthCodeIndex);
            IDataValidationConstraint EHealthCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "500");
            IDataValidation EHealthCodeLengthValidation = validationHelper.CreateValidation(EHealthCodeValidation, EHealthCodecell);
            EHealthCodeLengthValidation.ShowErrorBox = true;
            EHealthCodeLengthValidation.CreateErrorBox("", "Please Insert EHealthCode data from 1 to 100 characters");
            EHealthCodeLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(EHealthCodeLengthValidation);
            #endregion



            #region DataEffectiveDateFrom validation
            var FromDateIndex = DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateFrom").Index;
            CellRangeAddressList FromDateAddress = new CellRangeAddressList(1, 999999, FromDateIndex, FromDateIndex);
            IDataValidationConstraint FromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation FromDateValidation = validationHelper.CreateValidation(FromDateDate, FromDateAddress);
            FromDateValidation.ShowErrorBox = true;
            FromDateValidation.CreateErrorBox("wrong DataEffectiveDateFrom", "please enter right DataEffectiveDateFrom (yyyy-mm-dd)");
            FromDateValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(FromDateValidation);
            #endregion

            #region DataEffectiveDateTo validation
            var ToDateIndex = DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateTo").Index;
            CellRangeAddressList ToDateAddress = new CellRangeAddressList(1, 999999, ToDateIndex, ToDateIndex);
            IDataValidationConstraint ToDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation ToDateValidation = validationHelper.CreateValidation(ToDateDate, ToDateAddress);
            ToDateValidation.ShowErrorBox = true;
            ToDateValidation.CreateErrorBox("wrong DataEffectiveDateTo", "please enter right DataEffectiveDateTo (yyyy-mm-dd)");
            ToDateValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ToDateValidation);
            #endregion

            #region PriceDataEffectiveDateFrom validation
            var PriceFromDateIndex = DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateFrom").Index;
            CellRangeAddressList PriceFromDateAddress = new CellRangeAddressList(1, 999999, PriceFromDateIndex, PriceFromDateIndex);
            IDataValidationConstraint PriceFromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation PriceFromDateValidation = validationHelper.CreateValidation(PriceFromDateDate, PriceFromDateAddress);
            FromDateValidation.ShowErrorBox = true;
            FromDateValidation.CreateErrorBox("wrong Price EffectiveDateFrom", "please enter right Price EffectiveDateFrom (yyyy-mm-dd)");
            FromDateValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(PriceFromDateValidation);
            #endregion

            #region PriceDataEffectiveDateTo validation
            var PriceToDateIndex = DoctorFeesUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index;
            CellRangeAddressList PriceToDateAddress = new CellRangeAddressList(1, 999999, PriceToDateIndex, PriceToDateIndex);
            IDataValidationConstraint PriceToDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation PriceToDateValidation = validationHelper.CreateValidation(PriceToDateDate, PriceToDateAddress);
            PriceToDateValidation.ShowErrorBox = true;
            PriceToDateValidation.CreateErrorBox("wrong Price EffectiveDateTo", "please enter right Price EffectiveDateTo (yyyy-mm-dd)");
            PriceToDateValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(PriceToDateValidation);
            #endregion
        
        }
    }
}
