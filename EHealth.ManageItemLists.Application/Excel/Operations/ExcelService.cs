using AutoMapper;
using ClosedXML.Excel;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Excel.Operations
{
    public class ExcelService : IExcelService
    {
        private readonly IMapper _mapper;
   

        public ExcelService(IMapper mapper)
        {
            _mapper = mapper;
          
        }
        public ICell? GetCell(ISheet worksheet, int row,int CellIndex)
        {  
            return worksheet.GetRow(row).GetCell(CellIndex, MissingCellPolicy.RETURN_NULL_AND_BLANK);
          
        }
        public void CheckDupllicatesInFile(ISheet worksheet, int rowCounts, ref string dublicatedProperties, int row, int cellIndex, string dublicatedValue)
        {
            for (int i = 1; i <= rowCounts; i++)
            {
                if (i == row)
                {
                    continue;
                }
                if (GetCell(worksheet, row, cellIndex)?.ToString() == GetCell(worksheet, i, cellIndex)?.ToString())
                {
                    dublicatedProperties += dublicatedValue;/* "Duplicated UHIAId,";*/
                    break;
                }

            }
        }
        //public byte[] MapToExcel<T>(List<T> model, List<DropDownModel> dropdowns) where T : class
        public byte[] MapToExcel<T>(List<T> model) where T : class
        {
            DataTable data = ToDataTable(model);
            var filedata = SaveFilePhysically(data);
            using var stream = new MemoryStream();
            filedata.SaveAs(stream);
            var content = stream.ToArray();
            return content;
        }
        private DataTable ToDataTable<T>(List<T> excelRequests)
        {
            DataTable dataTable = new DataTable();

            string[] props = excelRequests.GetType().GetGenericArguments()[0].GetProperties().Select(p => p.Name).ToArray();

            CreateExcelColumns(dataTable, props);

            foreach (var request in excelRequests)
            {
                DataRow dataRow = dataTable.NewRow();
                FillDataRow(dataRow, request, props);
                dataTable.Rows.Add(dataRow);
            }

            dataTable.AcceptChanges();

            return dataTable;
        }

        private static void CreateExcelColumns(DataTable dataTable, string[] props)
        {
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop);
            }
        }
        public void CreateExcelFileFromStream(Stream file, string filePath)
        {
            XLWorkbook workbook = new XLWorkbook(file);
            workbook.SaveAs(filePath);
        }
        private static void FillDataRow<T>(DataRow dataRow, T form, string[] props)
        {
            for (int columnIndex = 0; columnIndex < props.Length; columnIndex++)
            {
                object cellValue = form.GetType().GetProperty(props[columnIndex]).GetValue(form);
                dataRow[columnIndex] = cellValue;
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
        {
            IXLStyle myCustomStyle = XLWorkbook.DefaultStyle;
            myCustomStyle.Font.SetBold(true);
            myCustomStyle.Font.SetFontSize(12);
            myCustomStyle.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            myCustomStyle.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            myCustomStyle.Alignment.WrapText = true;
            return myCustomStyle;
        }
        public (List<T> ValidData, List<N> InvalidData, int allRows, int failedRows) ReadExcelData<T, N>(Stream fileStream) where T : new()
        {
            List<T> data = new List<T>();
            List<N> invalidData = new List<N>();
            bool isRowValid = true;
            // Load the workbook from the file stream
            XLWorkbook workbook = new XLWorkbook(fileStream);
            // Assuming the data is present in the first sheet
            //IXLWorksheet sheet = workbook.Worksheets.FirstOrDefault() ?? throw new BadRequestException();
            IXLWorksheet sheet = workbook.Worksheets.FirstOrDefault() ?? throw new DataNotValidException();
            int rows = sheet.RowsUsed().Count();
            if (rows <= 1)
            {
                //throw new BadRequestException(messageAr: Messages.EmptyFileAr, messageEn: Messages.EmptyFileEn);
                throw new DataNotFoundException("EmptyFile");
            }
            // Get the header row to determine the property names
            IXLRow headerRow = sheet.Row(1);
            string[] props = GetColumNames(headerRow);
            ValidateSheetColumnsMatchClassProperties<T>(props);
            // Iterate through the rows and populate the data
            for (int rowIndex = 2; rowIndex <= sheet.RowsUsed().Count(); rowIndex++)
            {
                isRowValid = true;
                IXLRow dataRow = sheet.Row(rowIndex);
                T rowData = new T();

                for (int columnIndex = 1; columnIndex < props.Length + 1; columnIndex++)
                {
                    IXLCell dataCell = dataRow.Cell(columnIndex);
                    string propertyName = props[columnIndex - 1];

                    if (dataCell == null)
                        continue;
                    object cellValue = null;

                    // Handle different cell types
                    switch (dataCell.DataType)
                    {
                        case XLDataType.Number:
                            cellValue = dataCell.Value.GetNumber();
                            break;
                        case XLDataType.Text:
                            cellValue = dataCell.Value.ToString();
                            break;
                        case XLDataType.Boolean:
                            cellValue = dataCell.Value.GetBoolean();
                            break;
                            // Add more cases as per your requirement
                    }

                    isRowValid = SetValueToCorrespondingObjectProperty(isRowValid, rowData, propertyName, cellValue);
                }
                var rowValidationAgainstEntity = ValidateData(rowData);
                // Add the populated object to the data list or to invalid list
                AddTableRowToData(data, invalidData, isRowValid, rowData, rowValidationAgainstEntity);

            }
            return (ValidData: data, InvalidData: invalidData, allRows: rows - 1, failedRows: rows - (1 + data.Count));
        }

        private static bool SetValueToCorrespondingObjectProperty<T>(bool isRowValid, T rowData, string propertyName, object cellValue) where T : new()
        {
            // Set the value to the corresponding property of the object
            if (typeof(T).GetProperty(propertyName).PropertyType.Name.ToLower() == "string")
            {
                typeof(T).GetProperty(propertyName).SetValue(rowData, Convert.ToString(cellValue));
            }
            else if (typeof(T).GetProperty(propertyName).PropertyType.Name.ToLower() == "int32" && cellValue != null)
            {
                if (!int.TryParse(cellValue.ToString(), out int intValue))
                {
                    isRowValid = false;
                }
                else
                {
                    typeof(T).GetProperty(propertyName).SetValue(rowData, int.Parse(cellValue.ToString()));
                }
            }
            else if (typeof(T).GetProperty(propertyName).PropertyType.Name.ToLower() == "boolean" && cellValue != null)
            {
                if (!bool.TryParse(cellValue.ToString(), out bool boolValue))
                {
                    isRowValid = false;
                }
                else
                {
                    typeof(T).GetProperty(propertyName).SetValue(rowData, bool.Parse(cellValue.ToString()));
                }
            }

            return isRowValid;
        }

        private void AddTableRowToData<T, N>(List<T> data, List<N> invalidData, bool isRowValid, T rowData, string rowValidationAgainstEntity) where T : new()
        {
            if (isRowValid && rowValidationAgainstEntity.Length == 0)
            {
                data.Add(rowData);
            }
            else
            {
                StringBuilder message = new StringBuilder();
                if (!isRowValid)
                    message.Append("Data entered in a different format - ");
                if (rowValidationAgainstEntity.Length > 0)
                    message.Append(rowValidationAgainstEntity);
                N invalidDataObject = _mapper.Map<N>(rowData);
                typeof(N).GetProperty("Message").SetValue(invalidDataObject, message.ToString());
                invalidData.Add(invalidDataObject);
            }
        }

        private static void ValidateSheetColumnsMatchClassProperties<T>(string[] props) where T : new()
        {
            var validationProps = typeof(T).GetProperties().Select(p => p.Name).ToArray();
            if (!validationProps.SequenceEqual(props))
            {
                //throw new BadRequestException(messageAr: Messages.WrongFileFormatAr, messageEn: Messages.WrongFileEFormatEn);
                throw new DataNotValidException("WrongFileEFormat");
            }
        }

        private static string[] GetColumNames(IXLRow headerRow)
        {
            string[] props = new string[headerRow.CellsUsed().Count()];
            for (int columnIndex = 1; columnIndex < headerRow.CellsUsed().Count() + 1; columnIndex++)
            {
                IXLCell headerCell = headerRow.Cell(columnIndex);
                props[columnIndex - 1] = headerCell.Value.ToString();
            }

            return props;
        }

        private static string ValidateData<T>(T entity)
        {
            var result = "";
            StringBuilder message = new StringBuilder();
            List<ValidationResult> validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationErrors, true))
            {
                validationErrors.ForEach(error =>
                {
                    message.Append(string.Join(',', error.MemberNames));
                    message.Append(" : ");
                    message.Append(error.ErrorMessage);
                    message.AppendLine();
                });
                result = message.ToString();
            }
            return result;
        }

       
    }
}
