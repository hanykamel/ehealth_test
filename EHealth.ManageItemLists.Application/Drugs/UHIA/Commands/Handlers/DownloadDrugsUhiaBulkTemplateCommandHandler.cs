using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries;
using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Drugs.UHIA.Queries;
using EHealth.ManageItemLists.Application.Excel;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Handlers
{
    public class DownloadDrugsUhiaBulkTemplateCommandHandler : IRequestHandler<DownloadDrugsUhiaBulkTemplateCommand, byte[]>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IMediator _mediator;
        public DownloadDrugsUhiaBulkTemplateCommandHandler(IDrugsUHIARepository drugsUHIARepository, IMediator mediator)
        {
            _drugsUHIARepository = drugsUHIARepository;
            _mediator = mediator;
        }
        public async Task<byte[]> Handle(DownloadDrugsUhiaBulkTemplateCommand request, CancellationToken cancellationToken)
        {
            var drugUHIASearchQuery = new DrugUHIASearchQuery();

            drugUHIASearchQuery.ItemListId = request.ItemListId;
            drugUHIASearchQuery.EHealthCode = request.EHealthCode;
            drugUHIASearchQuery.LocalDrugCode = request.LocalDrugCode;
            drugUHIASearchQuery.InternationalNonProprietaryName = request.InternationalNonProprietaryName;
            drugUHIASearchQuery.ProprietaryName = request.ProprietaryName;
            drugUHIASearchQuery.DosageForm = request.DosageForm;
            drugUHIASearchQuery.RouteOfAdministration = request.RouteOfAdministration;
            drugUHIASearchQuery.OrderBy = request.OrderBy;
            drugUHIASearchQuery.Ascending = request.Ascending;

            var res = await _mediator.Send(drugUHIASearchQuery);
            res.Data = res.Data.Where(s => s.IsDeleted != true).ToList();
            //generate excel template 
            MemoryStream output = new MemoryStream();
            ExportToExcel<DrugsUHIADto> exportClass = new ExportToExcel<DrugsUHIADto>(_mediator)
            {
                Data = (List<DrugsUHIADto>)(res?.Data),
                Columns = DrugsUhiaTemplateHeader.Headers
            };
            var workbook = await exportClass.GenerateTemplate(request.ItemListSubtypeId);
            var basicSheet = (XSSFSheet)workbook.GetSheetAt(0);
            AddValidation(basicSheet);
            //fill data

            for (int i = 0; i < res.Data.Count(); i++)
            {
                var row = basicSheet.CreateRow(i + 1);
                var cell0 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthDrugCode").Index);
                cell0.SetCellValue(res.Data[i].EHealthCode);

                var cell1 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "LocalDrugCode").Index);
                cell1.SetCellValue(res.Data[i].LocalDrugCode);

                var cell2 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "InternationalNonProprietaryName").Index);
                cell2.SetCellValue(res.Data[i].InternationalNonProprietaryName);

                var cell3 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ProprietaryName").Index);
                cell3.SetCellValue(res.Data[i]?.ProprietaryName);

                var cell4 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DosageForm").Index);
                cell4.SetCellValue(res.Data[i]?.DosageForm);

                var cell5 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "RouteOfAdministration").Index);
                cell5.SetCellValue(res.Data[i]?.RouteOfAdministration);

                var cell6 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Manufacturer").Index);
                cell6.SetCellValue(res.Data[i]?.Manufacturer);

                var cell7 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MarketAuthorizationHolder").Index);
                cell7.SetCellValue(res.Data[i]?.MarketAuthorizationHolder);

                var cell8 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "RegistrationType").Index);
                cell8.SetCellValue(res.Data[i]?.RegistrationType?.RegistrationTypeEn);

                var cell9 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DrugsPackageType").Index);
                cell9.SetCellValue(res.Data[i]?.DrugsPackageType?.NameEn);

                var cell10 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MainUnit").Index);
                cell10.SetCellValue(res.Data[i]?.MainUnit?.UnitEn);

                var cell11 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NumberOfMainUnit").Index);
                cell11.SetCellValue(res.Data[i]?.NumberOfMainUnit?.ToString());

                var cell12 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubUnit").Index);
                cell12.SetCellValue(res.Data[i]?.SubUnit?.UnitEn);

                var cell13 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NumberOfSubunitPerMainUnit").Index);
                cell13.SetCellValue(res.Data[i]?.NumberOfSubunitPerMainUnit?.ToString());

                var cell14 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "TotalNumberSubunitsOfPack").Index);
                cell14.SetCellValue(res.Data[i]?.TotalNumberSubunitsOfPack?.ToString());

                var cell15 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ReimbursementCategory").Index);
                cell15.SetCellValue(res.Data[i]?.ReimbursementCategory?.NameEn);

                var cell16 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index);
                cell16.SetCellValue(res.Data[i]?.DataEffectiveDateFrom.ToString());

                var cell17 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index);
                cell17.SetCellValue(res.Data[i]?.DataEffectiveDateTo?.ToString());

                var cell18 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "MainUnitPrice").Index);
                cell18.SetCellValue(res.Data[i]?.DrugPrice?.MainUnitPrice.ToString());

                var cell19 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "FullPackPrice").Index);
                cell19.SetCellValue(res.Data[i]?.DrugPrice?.FullPackPrice.ToString());

                var cell20 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubUnitPrice").Index);
                cell20.SetCellValue(res.Data[i]?.DrugPrice?.SubUnitPrice.ToString());

                var cell21 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index);
                cell21.SetCellValue(res.Data[i]?.DrugPrice?.EffectiveDateFrom.ToString());

                var cell22 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index);
                cell22.SetCellValue(res.Data[i]?.DrugPrice?.EffectiveDateTo?.ToString());

                var cell23 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                cell23.SetCellValue(res.Data[i].Id.ToString());

                var cell24 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                cell24.SetCellValue(res.Data[i].ItemListId.ToString());

                var cell25 = row.CreateCell(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index + 3);
                cell25.SetCellValue(res.Data[i]?.DrugPrice?.Id.ToString());
            }
            //hide id and item list id columns
            basicSheet.SetColumnHidden(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1, true);
            basicSheet.SetColumnHidden(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2, true);
            basicSheet.SetColumnHidden(DrugsUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3, true);

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
            var EHealthCodeIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EHealthDrugCode").Index;
            CellRangeAddressList EHealthCodecell = new CellRangeAddressList(1, 999999, EHealthCodeIndex, EHealthCodeIndex);
            IDataValidationConstraint EHealthCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "2", "60");
            IDataValidation EHealthCodeLengthValidation = validationHelper.CreateValidation(EHealthCodeValidation, EHealthCodecell);
            EHealthCodeLengthValidation.ShowErrorBox = true;
            EHealthCodeLengthValidation.CreateErrorBox("", "Please Insert EHealthCode data from 2 to 60 characters");
            EHealthCodeLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(EHealthCodeLengthValidation);
            #endregion

            #region LocalDrugCode validation
            var LocalDrugCodeIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "LocalDrugCode").Index;
            CellRangeAddressList LocalDrugCodecell = new CellRangeAddressList(1, 999999, LocalDrugCodeIndex, LocalDrugCodeIndex);
            IDataValidationConstraint LocalDrugCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "2", "60");
            IDataValidation LocalDrugCodeLengthValidation = validationHelper.CreateValidation(LocalDrugCodeValidation, LocalDrugCodecell);
            LocalDrugCodeLengthValidation.ShowErrorBox = true;
            LocalDrugCodeLengthValidation.CreateErrorBox("", "Please Insert LocalDrugCode data from 2 to 60 characters");
            LocalDrugCodeLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(LocalDrugCodeLengthValidation);
            #endregion

            #region ProprietaryName validation
            var ProprietaryNameIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "ProprietaryName").Index;
            CellRangeAddressList ProprietaryNamecell = new CellRangeAddressList(1, 999999, ProprietaryNameIndex, ProprietaryNameIndex);
            IDataValidationConstraint ProprietaryNameValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "4", "270");
            IDataValidation ProprietaryNameLengthValidation = validationHelper.CreateValidation(ProprietaryNameValidation, ProprietaryNamecell);
            ProprietaryNameLengthValidation.ShowErrorBox = true;
            ProprietaryNameLengthValidation.CreateErrorBox("", "Please Insert ProprietaryName data from 4 to 270 characters");
            ProprietaryNameLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(ProprietaryNameLengthValidation);
            #endregion


            #region DosageForm validation
            var DosageFormIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DosageForm").Index;
            CellRangeAddressList DosageFormcell = new CellRangeAddressList(1, 999999, DosageFormIndex, DosageFormIndex);
            IDataValidationConstraint DosageFormValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "3", "280");
            IDataValidation DosageFormLengthValidation = validationHelper.CreateValidation(DosageFormValidation, DosageFormcell);
            DosageFormLengthValidation.ShowErrorBox = true;
            DosageFormLengthValidation.CreateErrorBox("", "Please Insert DosageForm data from 3 to 280 characters");
            DosageFormLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(DosageFormLengthValidation);
            #endregion


            #region RouteOfAdministration validation
            var RouteOfAdministrationIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "RouteOfAdministration").Index;
            CellRangeAddressList RouteOfAdministrationcell = new CellRangeAddressList(1, 999999, RouteOfAdministrationIndex, RouteOfAdministrationIndex);
            IDataValidationConstraint RouteOfAdministrationValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "2", "150");
            IDataValidation RouteOfAdministrationLengthValidation = validationHelper.CreateValidation(RouteOfAdministrationValidation, RouteOfAdministrationcell);
            RouteOfAdministrationLengthValidation.ShowErrorBox = true;
            RouteOfAdministrationLengthValidation.CreateErrorBox("", "Please Insert RouteOfAdministration data from 2 to 150 characters");
            RouteOfAdministrationLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(RouteOfAdministrationLengthValidation);
            #endregion


            #region Manufacturer validation
            var ManufacturerIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "Manufacturer").Index;
            CellRangeAddressList Manufacturercell = new CellRangeAddressList(1, 999999, ManufacturerIndex, ManufacturerIndex);
            IDataValidationConstraint ManufacturerValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "4", "270");
            IDataValidation ManufacturerLengthValidation = validationHelper.CreateValidation(ManufacturerValidation, Manufacturercell);
            ManufacturerLengthValidation.ShowErrorBox = true;
            ManufacturerLengthValidation.CreateErrorBox("", "Please Insert Manufacturer data from 4 to 270 characters");
            ManufacturerLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ManufacturerLengthValidation);
            #endregion

            #region MarketAuthorizationHolder validation
            var MarketAuthorizationHolderIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "MarketAuthorizationHolder").Index;
            CellRangeAddressList MarketAuthorizationHoldercell = new CellRangeAddressList(1, 999999, MarketAuthorizationHolderIndex, MarketAuthorizationHolderIndex);
            IDataValidationConstraint MarketAuthorizationHolderValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "4", "270");
            IDataValidation MarketAuthorizationHolderLengthValidation = validationHelper.CreateValidation(MarketAuthorizationHolderValidation, MarketAuthorizationHoldercell);
            MarketAuthorizationHolderLengthValidation.ShowErrorBox = true;
            MarketAuthorizationHolderLengthValidation.CreateErrorBox("", "Please Insert MarketAuthorizationHolder data from 4 to 270 characters");
            MarketAuthorizationHolderLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(MarketAuthorizationHolderLengthValidation);
            #endregion

            #region NumberOfMainUnit validation
            var NumberOfMainUnitIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "NumberOfMainUnit").Index;
            CellRangeAddressList NumberOfMainUnitcell = new CellRangeAddressList(1, 999999, NumberOfMainUnitIndex, NumberOfMainUnitIndex);
            IDataValidationConstraint NumberOfMainUnitValidation = validationHelper.CreateintConstraint(OperatorType.BETWEEN, "1", "1000");
            IDataValidation NumberOfMainUnitLengthValidation = validationHelper.CreateValidation(NumberOfMainUnitValidation, NumberOfMainUnitcell);
            NumberOfMainUnitLengthValidation.ShowErrorBox = true;
            NumberOfMainUnitLengthValidation.CreateErrorBox("", "Please Insert NumberOfMainUnit data from 1 to 1000 characters");
            NumberOfMainUnitLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(NumberOfMainUnitLengthValidation);
            #endregion

            #region  NumberOfSubunitPerMainUni validation
            var NumberOfSubunitPerMainUnitIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "NumberOfSubunitPerMainUnit").Index;
            CellRangeAddressList NumberOfSubunitPerMainUnitcell = new CellRangeAddressList(1, 999999, NumberOfSubunitPerMainUnitIndex, NumberOfSubunitPerMainUnitIndex);
            IDataValidationConstraint NumberOfSubunitPerMainUnitValidation = validationHelper.CreateintConstraint(OperatorType.BETWEEN, "1", "20");
            IDataValidation NumberOfSubunitPerMainUnitLengthValidation = validationHelper.CreateValidation(NumberOfSubunitPerMainUnitValidation, NumberOfSubunitPerMainUnitcell);
            NumberOfSubunitPerMainUnitLengthValidation.ShowErrorBox = true;
            NumberOfSubunitPerMainUnitLengthValidation.CreateErrorBox("", "Please Insert NumberOfSubunitPerMainUnit data from 1 to 20 characters");
            NumberOfSubunitPerMainUnitLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(NumberOfSubunitPerMainUnitLengthValidation);
            #endregion

            #region  TotalNumberSubunitsOfPack validation
            var TotalNumberSubunitsOfPackIndex = DrugsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "TotalNumberSubunitsOfPack").Index;
            CellRangeAddressList TotalNumberSubunitsOfPackcell = new CellRangeAddressList(1, 999999, TotalNumberSubunitsOfPackIndex, TotalNumberSubunitsOfPackIndex);
            IDataValidationConstraint TotalNumberSubunitsOfPackValidation = validationHelper.CreateintConstraint(OperatorType.BETWEEN, "1", "20");
            IDataValidation TotalNumberSubunitsOfPackValidationLengthValidation = validationHelper.CreateValidation(TotalNumberSubunitsOfPackValidation, TotalNumberSubunitsOfPackcell);
            TotalNumberSubunitsOfPackValidationLengthValidation.ShowErrorBox = true;
            TotalNumberSubunitsOfPackValidationLengthValidation.CreateErrorBox("", "Please Insert TotalNumberSubunitsOfPack data from 1 to 20 characters");
            TotalNumberSubunitsOfPackValidationLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(TotalNumberSubunitsOfPackValidationLengthValidation);
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
