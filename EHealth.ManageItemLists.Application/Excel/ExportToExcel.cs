using ClosedXML.Excel;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.Queries;
using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.Queries;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.Queries;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.Queries;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.Queries;
using EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.Queries;
using EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.Queries;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.Queries;
using EHealth.ManageItemLists.Application.Lookups.Subtype.Queries;
using EHealth.ManageItemLists.Application.Lookups.Type.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitRooms.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitsTypes.Queries;
using EHealth.ManageItemLists.Domain.Shared.BulkUpload;
using MediatR;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Data;

namespace EHealth.ManageItemLists.Application.Excel
{
    public class ExportToExcel<T>
    {
        //public string[] Columns { get; set; }
        public List<HeaderItem> Columns { get; set; }
        public List<T> Data { get; set; }
        private readonly IMediator _mediator;
        public ExportToExcel(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        private static void CreateExcelColumns(DataTable dataTable, string[] props)
        {
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop);
            }
        }

        private static void FillDataRow<T>(DataRow dataRow, T form, string[] props)
        {
            for (int columnIndex = 0; columnIndex < props.Length; columnIndex++)
            {
                object cellValue = form.GetType().GetProperty(props[columnIndex]).GetValue(form);
                var cellType = cellValue.GetType();
                if (cellType.IsClass && cellType != typeof(string))
                {

                }
                else
                {
                    dataRow[columnIndex] = cellValue;
                }
            }
        }

        private XLWorkbook SaveFilePhysically(DataTable dataTable)
        {
            XLWorkbook workbook = new XLWorkbook();

            workbook.Worksheets.Add(dataTable, "Sheet 1");
            workbook.Worksheet("Sheet 1").ColumnWidth = 18;

            workbook.Worksheet("Sheet 1").Columns(27, 28).Style = SetColumnsStyle();
            workbook.Worksheet("Sheet 1").Range("A1", "AB1").Style = SetSheetStyle();
            workbook.Worksheet("Sheet 1").Columns().AdjustToContents();
            return workbook;
        }


        private static IXLStyle SetColumnsStyle()
        {
            IXLStyle myCustomStyle = XLWorkbook.DefaultStyle;
            myCustomStyle.Alignment.WrapText = true;
            return myCustomStyle;
        }

        private static IXLStyle SetSheetStyle()
        {   ///// ADD the style file we have 
            IXLStyle myCustomStyle = XLWorkbook.DefaultStyle;
            myCustomStyle.Font.SetBold(true);
            myCustomStyle.Font.SetFontSize(12);
            myCustomStyle.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            myCustomStyle.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            myCustomStyle.Alignment.WrapText = true;
            return myCustomStyle;
        }
        public async Task<IWorkbook> GenerateTemplate(int itemListSubtypeId)
        {
            MemoryStream output = new MemoryStream();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            var basicSheet = (XSSFSheet)workbook.GetSheetAt(0);
            var props = Columns;
            //create header
            var header = sheet.CreateRow(0);
            for (var i = 0; i < props.Count(); i++)
            {
                var cell = header.CreateCell(i);
                cell.SetCellValue(props[i].TitleEn);
           
              
            }

            var lookupsProp = props.Where(p => p.Lookup==true).ToList();
            
            XSSFCellStyle headerStyle = ExcelStyle.SetWorkbookStyle(workbook);
          
            for (int i = 0; i < header.Cells.Count; i++)
            {
                sheet.AutoSizeColumn(i);
                header.Cells[i].CellStyle = headerStyle;
            }

            foreach (var lookup in lookupsProp)
            {
              
                var lookupSheet = workbook.CreateSheet(lookup.Key);
                var options = await GetLookupOptions(lookup.Key, itemListSubtypeId);

                for (int i = 0, length = options.Count(); i < length; i++)
                {
                    var title = options[i];
                    var row = lookupSheet.CreateRow(i);
                    var cell = row.CreateCell(0);
                    cell.SetCellValue(title);
                }
                IDataValidationHelper validationHelper = new XSSFDataValidationHelper(basicSheet);
                CellRangeAddressList lookupcell = new CellRangeAddressList(1, 999999, lookup.Index, lookup.Index);
                IDataValidationConstraint optionsValidation = validationHelper.CreateFormulaListConstraint($"{lookup.Key}!$A$1:$A$" + options.Count());
                IDataValidation lookupValidation = validationHelper.CreateValidation(optionsValidation, lookupcell);
                lookupValidation.SuppressDropDownArrow = true;
                basicSheet.AddValidationData(lookupValidation);
                workbook.SetSheetHidden(workbook.GetSheetIndex(lookupSheet), SheetState.Hidden);

            }
            return workbook;

        }


        private async Task<List<string>> GetLookupOptions(string type,int itemListSubtypeId)
        {
            switch (type)
            {
                case "Category":
                    {
                        
                        var categories = await _mediator.Send(new CategoriesSearchQuery { EnablePagination = false ,ItemListSubtypeId=itemListSubtypeId }) ;
                        return categories.Data.Select(c => c.CategoryEn.ToString()).ToList();
                    }

                case "SubCategory":
                    {
                        var subCategories = await _mediator.Send(new SubCategoriesSearchQuery { EnablePagination = false, ItemListSubtypeId = itemListSubtypeId });
                        return subCategories.Data.Select(c => c.SubCategoryEn).ToList();
                    }
                case "LocalUnitOfMeasure":
                    {
                        var localUnitOfMeasures = await _mediator.Send(new LocalUnitOfMeasuresSearchQuery { EnablePagination = false });
                        return localUnitOfMeasures.Data.Select(c => c.MeasureTypeEn).ToList();
                    }
                case "UnitRoom":
                    {
                        var unitRooms = await _mediator.Send(new UnitRoomsSearchQuery { EnablePagination = false });
                        return unitRooms.Data.Select(c => c.NameEN).ToList();
                    }
                case "PackageComplexityClassification":
                    {
                        var packageComplexity = await _mediator.Send(new PackageComplexityClassificationSearchQuery { EnablePagination = false });
                        return packageComplexity.Data.Select(c => c.ComplexityEn).ToList();
                    }
                case "UnitOfDoctorFees":
                    {
                        var unitOfDoctorFees = await _mediator.Send(new UnitOfTheDoctorFeesSearchQuery { EnablePagination = false });
                        return unitOfDoctorFees.Data.Select(c => c.NameEN).ToList();
                    } 
                case "RegistrationType":
                    {
                        var registrationTypes = await _mediator.Send(new RegistrationTypesSearchQuery { EnablePagination = false });
                        return registrationTypes.Data.Select(c => c.RegistrationTypeEn).ToList();
                    }
                case "DrugsPackageType":
                    {
                        var drugsPackageTypes = await _mediator.Send(new DrugsPackageTypesSearchQuery { EnablePagination = false });
                        return drugsPackageTypes.Data.Select(c => c.NameEn).ToList();
                    }
                case "MainUnit":
                    {
                        var unitTypes = await _mediator.Send(new UnitsTypesSearchQuery { EnablePagination = false });
                        return unitTypes.Data.Select(c => c.UnitEn).ToList();
                    }
                case "SubUnit":
                    {
                        var unitTypes = await _mediator.Send(new UnitsTypesSearchQuery { EnablePagination = false });
                        return unitTypes.Data.Select(c => c.UnitEn).ToList(); 
                    }
                case "ReimbursementCategory":
                    {
                        var reimbursementCategory = await _mediator.Send(new ReimbursementCategoriesSearchQuery { EnablePagination = false });
                        return reimbursementCategory.Data.Select(c => c.NameEn).ToList(); 
                    }
                case "LocalSpecialtyDepartment":
                    {
                        var LocalSpeciality = await _mediator.Send(new LocalSpecialtyDepartmentsSearchQuery { EnablePagination = false });
                        return LocalSpeciality.Data.Select(c => c.LocalSpecialityEn).ToList();
                    }

                case "PriceUnit":
                    {
                        var PriceUnit = await _mediator.Send(new PriceUnitSearchQuery { EnablePagination = false });
                        return PriceUnit.Data.Select(c => c.NameEN).ToList();
                    }
                case "ItemType":
                    {
                        var itemType = await _mediator.Send(new TypesSearchQuery { EnablePagination = false });
                        return itemType.Data.Select(c => c.NameEN).ToList();
                    }

                case "ItemListSubtype":
                    {
                        var PriceUnit = await _mediator.Send(new SubTypeSearchQuery { EnablePagination = false });
                        return PriceUnit.Data.Select(c => c.NameEN).ToList();
                    }
                default:
                    {
                        return new List<string>(); 
                    }
            }
        }


    }
    
}
