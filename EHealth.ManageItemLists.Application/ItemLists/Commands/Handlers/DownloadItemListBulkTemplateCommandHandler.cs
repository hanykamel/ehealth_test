using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries;
using EHealth.ManageItemLists.Application.Excel;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Application.ItemLists.Queries;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands.Handlers
{
    public class DownloadItemListBulkTemplateCommandHandler : IRequestHandler<DownloadItemListBulkTemplateCommand, byte[]>
    {
        private readonly IItemListRepository  _itemListRepository ;
        private readonly IMediator _mediator;
        public DownloadItemListBulkTemplateCommandHandler(IItemListRepository itemListRepository,IMediator mediator)
        {
            _itemListRepository = itemListRepository;
            _mediator = mediator;
        }

        public async Task<byte[]> Handle(DownloadItemListBulkTemplateCommand request, CancellationToken cancellationToken)
        {
            var searchItemListQuery = new SearchItemListQuery();

            searchItemListQuery.Code = request.Code;
            searchItemListQuery.NameEN = request.NameEN;
            searchItemListQuery.NameAr = request.NameAr;
            searchItemListQuery.ItemListSubtypeId = request.ItemListSubtypeId;
            searchItemListQuery.ItemListTypeId = request.ItemListTypeId;
            searchItemListQuery.OrderBy = request.OrderBy;
            searchItemListQuery.Ascending = request.Ascending;

            var res = await _mediator.Send(searchItemListQuery);
            res.Data = res.Data.Where(s => s.IsDeleted != true).ToList();
            //generate excel template 
            MemoryStream output = new MemoryStream();
            ExportToExcel<ItemListDto> exportClass = new ExportToExcel<ItemListDto>(_mediator)
            {
                Data = (List<ItemListDto>)(res?.Data),
                Columns = ItemListTemplateHeader.Headers
            };
            var workbook = await exportClass.GenerateTemplate(request.ItemListSubtypeId??0);
            var basicSheet = (XSSFSheet)workbook.GetSheetAt(0);


            AddValidation(basicSheet);
            //fill data

            for (int i = 0; i < res.Data.Count(); i++)
            {
                var row = basicSheet.CreateRow(i + 1);
                var cell0 = row.CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "Code").Index);
                cell0.SetCellValue(res.Data[i].Code);
                var style = workbook.CreateCellStyle();
                style.IsLocked=true;
                cell0.CellStyle=style;

                var cell1 = row.CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NameAr").Index);
                cell1.SetCellValue(res.Data[i].NameAr);
                var cell2 = row.CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "NameEN").Index);
                cell2.SetCellValue(res.Data[i].NameEN);
                var cell3 = row.CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemType").Index);
                cell3.SetCellValue(res.Data[i]?.itemListType?.NameEN);
                var cell4 = row.CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index);
                cell4.SetCellValue(res.Data[i]?.itemListSubtype?.NameEN);
       
                var cell5 = row.CreateCell(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 1);
                cell5.SetCellValue(res.Data[i].Id.ToString());
       
            }
            //hide id and item list id columns
            basicSheet.SetColumnHidden(ItemListTemplateHeader.Headers.FirstOrDefault(c => c.Key == "ItemListSubtype").Index + 1, true);


            //adjust column width
            var header = basicSheet.GetRow(0);
            for (int i = 0; i < header.Cells.Count; i++)
            {
                basicSheet.AutoSizeColumn(i);
            }
            basicSheet.ProtectSheet("");
            workbook.Write(output);
            return output.ToArray();
        }
        public void AddValidation(XSSFSheet basicSheet)
        {
            IDataValidationHelper validationHelper = new XSSFDataValidationHelper(basicSheet);
            #region EHealthCode validation
            var EHealthCodeIndex = ItemListTemplateHeader.Headers.FirstOrDefault(p => p.Key == "Code").Index;
            CellRangeAddressList EHealthCodecell = new CellRangeAddressList(1, 999999, EHealthCodeIndex, EHealthCodeIndex);
            IDataValidationConstraint EHealthCodeValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "500");
            IDataValidation EHealthCodeLengthValidation = validationHelper.CreateValidation(EHealthCodeValidation, EHealthCodecell);
            EHealthCodeLengthValidation.ShowErrorBox = true;
            EHealthCodeLengthValidation.CreateErrorBox("", "Please Insert Code data from 1 to 100 characters");
            EHealthCodeLengthValidation.EmptyCellAllowed = true;
            
            basicSheet.AddValidationData(EHealthCodeLengthValidation);
            #endregion

            #region ShortDescAr validation
            var ShortDescArIndex = ItemListTemplateHeader.Headers.FirstOrDefault(p => p.Key == "NameAr").Index;
            CellRangeAddressList ShortDescArcell = new CellRangeAddressList(1, 999999, ShortDescArIndex, ShortDescArIndex);
            IDataValidationConstraint ShortDescArValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "1000");
            IDataValidation ShortDescArLengthValidation = validationHelper.CreateValidation(ShortDescArValidation, ShortDescArcell);
            ShortDescArLengthValidation.ShowErrorBox = true;
            ShortDescArLengthValidation.CreateErrorBox("", "Please Insert NameAr data from 1 to 1000 characters");
            ShortDescArLengthValidation.EmptyCellAllowed = true;
            basicSheet.AddValidationData(ShortDescArLengthValidation);
            #endregion

            #region ShortDescEn validation
            var ShortDescEnIndex = ItemListTemplateHeader.Headers.FirstOrDefault(p => p.Key == "NameEN").Index;
            CellRangeAddressList ShortDescEncell = new CellRangeAddressList(1, 999999, ShortDescEnIndex, ShortDescEnIndex);
            IDataValidationConstraint ShortDescEnValidation = validationHelper.CreateTextLengthConstraint(OperatorType.BETWEEN, "1", "1000");
            IDataValidation ShortDescEnLengthValidation = validationHelper.CreateValidation(ShortDescEnValidation, ShortDescEncell);
            ShortDescEnLengthValidation.ShowErrorBox = true;
            ShortDescEnLengthValidation.CreateErrorBox("", "Please Insert NameEN data from 1 to 1000 characters");
            ShortDescEnLengthValidation.EmptyCellAllowed = false;
            basicSheet.AddValidationData(ShortDescEnLengthValidation);
            #endregion

        }
    }
}
