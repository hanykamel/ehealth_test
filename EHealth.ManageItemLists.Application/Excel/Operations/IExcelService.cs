using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Excel.Operations
{
    public interface IExcelService
    {
        (List<T> ValidData, List<N> InvalidData, int allRows, int failedRows) ReadExcelData<T, N>(Stream fileStream) where T : new();
        byte[] MapToExcel<T>(List<T> model) where T : class;
        ICell? GetCell(ISheet worksheet, int row, int CellIndex);
        void CheckDupllicatesInFile(ISheet worksheet, int rowCounts, ref string dublicatedProperties, int row, int cellIndex, string dublicatedValue);
    }
}
