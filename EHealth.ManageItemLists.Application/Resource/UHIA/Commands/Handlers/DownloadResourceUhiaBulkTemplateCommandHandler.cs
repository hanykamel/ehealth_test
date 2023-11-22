using EHealth.ManageItemLists.Application.Excel;
using EHealth.ManageItemLists.Application.Procedure.ICHI.Commands;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Application.Procedure.ICHI.Queries;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Handlers
{
    public class DownloadResourceUhiaBulkTemplateCommandHandler : IRequestHandler<DownloadResourceUhiaBulkTemplateCommand, byte[]>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository ;
        private readonly IMediator _mediator;
        public DownloadResourceUhiaBulkTemplateCommandHandler(IResourceUHIARepository resourceUHIARepository, IMediator mediator)
        {
            _resourceUHIARepository = resourceUHIARepository ;
            _mediator = mediator ;
        }
        public async Task<byte[]> Handle(DownloadResourceUhiaBulkTemplateCommand request, CancellationToken cancellationToken)
        {
            var resourceUHIASearchQuery = new ResourceUHIASearchQuery();

            resourceUHIASearchQuery.ItemListId = request.ItemListId;
            resourceUHIASearchQuery.Code = request.Code;
            resourceUHIASearchQuery.DescriptorAr = request.DescriptorAr;
            resourceUHIASearchQuery.DescriptorEn = request.DescriptorEn;
            resourceUHIASearchQuery.OrderBy = request.OrderBy;
            resourceUHIASearchQuery.Ascending = request.Ascending;

            var res = await _mediator.Send(resourceUHIASearchQuery);
            res.Data = res.Data.Where(s => s.IsDeleted != true).ToList();
            //generate excel template 
            MemoryStream output = new MemoryStream();
            ExportToExcel<ResourceUHIADto> exportClass = new ExportToExcel<ResourceUHIADto>(_mediator)
            {
                Data = (List<ResourceUHIADto>)(res?.Data),
                Columns = ResourceUhiaTemplateHeader.Headers
            };
            var workbook = await exportClass.GenerateTemplate(request.ItemListSubtypeId);
            var basicSheet = (XSSFSheet)workbook.GetSheetAt(0);
            AddValidation(basicSheet);
            //fill data

            for (int i = 0; i < res.Data.Count(); i++)
            {
                var row = basicSheet.CreateRow(i + 1);
                var cell0 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EHealthCode").Index);
                cell0.SetCellValue(res.Data[i].EHealthCode);
                var cell1 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorAr").Index);
                cell1.SetCellValue(res.Data[i].DescriptorAr);
                var cell2 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DescriptorEn").Index);
                cell2.SetCellValue(res.Data[i].DescriptorEn);
                var cell3 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Category").Index);
                cell3.SetCellValue(res.Data[i].Category?.DefinitionEn);

                var cell4 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index);
                cell4.SetCellValue(res.Data[i]?.SubCategory?.SubCategoryEn);

                var cell5 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "SubCategory").Index);
                cell5.SetCellValue(res.Data[i]?.SubCategory?.SubCategoryEn);

                var cell6 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateFrom").Index);
                cell6.SetCellValue(res.Data[i]?.DataEffectiveDateFrom.ToString());

                var cell7 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index);
                cell7.SetCellValue(res.Data[i]?.DataEffectiveDateTo?.ToString());

                var cell8 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "DataEffectiveDateTo").Index);
                cell8.SetCellValue(res.Data[i]?.DataEffectiveDateTo?.ToString());

                var cell9 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Price").Index);
                cell9.SetCellValue(res.Data[i]?.ItemListPrice?.Price.ToString());

                var cell10 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "PriceUnit").Index);
                cell10.SetCellValue(res.Data[i]?.ItemListPrice?.PriceUnit?.NameEN);

                var cell11 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateFrom").Index);
                cell11.SetCellValue(res.Data[i]?.ItemListPrice?.EffectiveDateFrom.ToString());
                var cell12 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index);
                cell12.SetCellValue(res.Data[i]?.ItemListPrice?.EffectiveDateTo?.ToString());
                var cell13 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1);
                cell13.SetCellValue(res.Data[i].Id.ToString());
                var cell14 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2);
                cell14.SetCellValue(res.Data[i].ItemListId.ToString());
                var cell15 = row.CreateCell(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index + 3);
                cell15.SetCellValue(res.Data[i]?.ItemListPrice?.Id.ToString());
            }
            //hide id and item list id columns
            basicSheet.SetColumnHidden(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 1, true);
            basicSheet.SetColumnHidden(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 2, true);
            basicSheet.SetColumnHidden(ResourceUhiaTemplateHeader.Headers.FirstOrDefault(c => c.Key == "EffectiveDateTo").Index + 3, true);

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
            var EHealthCodeIndex = ResourceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EHealthCode").Index;
            CellRangeAddressList EHealthCodecell = new CellRangeAddressList(1, 999999, EHealthCodeIndex, EHealthCodeIndex);
            IDataValidationConstraint EHealthCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "500");
            IDataValidation EHealthCodeLengthValidation = validationHelper.CreateValidation(EHealthCodeValidation, EHealthCodecell);
            EHealthCodeLengthValidation.ShowErrorBox = true;
            EHealthCodeLengthValidation.CreateErrorBox("", "Please Insert data from 1 to 500 characters");
            EHealthCodeLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(EHealthCodeLengthValidation);
            #endregion

            #region ShortDescAr validation
            var ShortDescArIndex = DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DescriptorAr").Index;
            CellRangeAddressList ShortDescArcell = new CellRangeAddressList(1, 999999, ShortDescArIndex, ShortDescArIndex);
            IDataValidationConstraint ShortDescArValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "100");
            IDataValidation ShortDescArLengthValidation = validationHelper.CreateValidation(ShortDescArValidation, ShortDescArcell);
            ShortDescArLengthValidation.ShowErrorBox = true;
            ShortDescArLengthValidation.CreateErrorBox("", "Please Insert DescriptorAr data from 1 to 100 characters");
            ShortDescArLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(ShortDescArLengthValidation);
            #endregion

            #region ShortDescEn validation
            var ShortDescEnIndex = DevsAndAssetsUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DescriptorEn").Index;
            CellRangeAddressList ShortDescEncell = new CellRangeAddressList(1, 999999, ShortDescEnIndex, ShortDescEnIndex);
            IDataValidationConstraint ShortDescEnValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "100");
            IDataValidation ShortDescEnLengthValidation = validationHelper.CreateValidation(ShortDescEnValidation, ShortDescEncell);
            ShortDescEnLengthValidation.ShowErrorBox = true;
            ShortDescEnLengthValidation.CreateErrorBox("", "Please Insert DescriptorEn data from 1 to 100 characters");
            ShortDescEnLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(ShortDescEnLengthValidation);
            #endregion


            #region DataEffectiveDateFrom validation
            var FromDateIndex = ResourceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateFrom").Index;
            CellRangeAddressList FromDateAddress = new CellRangeAddressList(1, 999999, FromDateIndex, FromDateIndex);
            IDataValidationConstraint FromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation FromDateValidation = validationHelper.CreateValidation(FromDateDate, FromDateAddress);
            FromDateValidation.ShowErrorBox = true;
            FromDateValidation.CreateErrorBox("wrong DataEffectiveDateFrom", "please enter right DataEffectiveDateFrom (yyyy-mm-dd)");
            FromDateValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(FromDateValidation);
            #endregion

            #region DataEffectiveDateTo validation
            var ToDateIndex = ResourceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "DataEffectiveDateTo").Index;
            CellRangeAddressList ToDateAddress = new CellRangeAddressList(1, 999999, ToDateIndex, ToDateIndex);
            IDataValidationConstraint ToDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation ToDateValidation = validationHelper.CreateValidation(ToDateDate, ToDateAddress);
            ToDateValidation.ShowErrorBox = true;
            ToDateValidation.CreateErrorBox("wrong DataEffectiveDateTo", "please enter right DataEffectiveDateTo (yyyy-mm-dd)");
            ToDateValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ToDateValidation);
            #endregion

            #region PriceDataEffectiveDateFrom validation
            var PriceFromDateIndex = ResourceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateFrom").Index;
            CellRangeAddressList PriceFromDateAddress = new CellRangeAddressList(1, 999999, PriceFromDateIndex, PriceFromDateIndex);
            IDataValidationConstraint PriceFromDateDate = validationHelper.CreateDateConstraint(OperatorType.BETWEEN, "=DATE(1900,1,1)", "=DATE(2119,12,31)", "yyyy-mm-dd");
            IDataValidation PriceFromDateValidation = validationHelper.CreateValidation(PriceFromDateDate, PriceFromDateAddress);
            FromDateValidation.ShowErrorBox = true;
            FromDateValidation.CreateErrorBox("wrong Price EffectiveDateFrom", "please enter right Price EffectiveDateFrom (yyyy-mm-dd)");
            FromDateValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(PriceFromDateValidation);
            #endregion

            #region PriceDataEffectiveDateTo validation
            var PriceToDateIndex = ResourceUhiaTemplateHeader.Headers.FirstOrDefault(p => p.Key == "EffectiveDateTo").Index;
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
