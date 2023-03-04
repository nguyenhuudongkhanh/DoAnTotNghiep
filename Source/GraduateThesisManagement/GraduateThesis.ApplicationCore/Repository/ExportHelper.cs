using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ExtensionMethods;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

internal class ExportHelper<TOutput>
{
    public ExportHelper()
    {
    }

    public ICellStyle SetDefaultCellStyle(IWorkbook workbook, bool isHeaderStyle)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();
        IFont font = workbook.CreateFont();

        font.FontName = "Time New Roman";
        font.FontHeight = 30 * 20;
        font.FontHeightInPoints = 14;

        if (isHeaderStyle)
            font.IsBold = true;

        cellStyle.SetFont(font);

        return cellStyle;
    }

    public void SetSheetHeader(ISheet sheet, ICellStyle cellStyle, string[] includeProperties)
    {
        IRow headerRow = sheet.CreateRow(0);

        PropertyInfo[] outputProperties = typeof(TOutput).GetProperties();
        int cellIndex = 0;
        foreach (PropertyInfo propertyInfo in outputProperties)
        {
            if (includeProperties.Any(i => i == propertyInfo.Name))
            {
                ICell cell = headerRow.CreateCell(cellIndex);
                DisplayAttribute displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                    cell.SetCellValue(propertyInfo.Name);
                else
                    cell.SetCellValue(displayAttribute.Name);

                cell.CellStyle = cellStyle;
                cellIndex++;
            }
        }

        for (int i = 0; i < outputProperties.Length; i++)
        {
            sheet.AutoSizeColumn(i);
        }
    }

    public void SetDataToSheet(ISheet sheet, ICellStyle cellStyle, IEnumerable<TOutput> outputs, string[] includeProperties)
    {
        int rowIndex = 1;
        int cellIndex = 0;
        Type outputType = typeof(TOutput);
        PropertyInfo[] outputProperties = outputType.GetProperties();

        foreach (TOutput output in outputs)
        {
            IRow row = sheet.CreateRow(rowIndex);
            cellIndex = 0;
            foreach (PropertyInfo propertyInfo in outputProperties)
            {
                if (includeProperties.Any(i => i == propertyInfo.Name))
                {
                    ICell cell = row.CreateCell(cellIndex);
                    cell.CellStyle = cellStyle;
                    SetCellValue(cell, propertyInfo.GetValue(output));

                    cellIndex++;
                }
            }
            rowIndex++;
        }

        for (int i = 0; i < outputProperties.Length; i++)
        {
            sheet.AutoSizeColumn(i);
        }
    }

    public void SetCellValue(ICell cell, object value)
    {
        if (value.IsString())
            cell.SetCellValue(value.ToString());
        else if (value.IsBool())
            cell.SetCellValue((bool)value);
        else if (value.IsNumber())
            cell.SetCellValue(value.ToString());
        else
            cell.SetCellValue(value.ToString());
    }

    public IWorkbook InitWorkbook(ExportTypeOptions exportTypeOptions)
    {
        if (exportTypeOptions == ExportTypeOptions.XLS)
            return new HSSFWorkbook();

        return new XSSFWorkbook();
    }

    public MemoryStream CreateExportStream(IEnumerable<TOutput> outputs, ExportTypeOptions exportTypeOptions, string sheetName, string[] includeProperties)
    {
        IWorkbook workbook = null;
        try
        {
            workbook = InitWorkbook(exportTypeOptions);

            ICellStyle headerCellStyle = SetDefaultCellStyle(workbook, true);
            ICellStyle cellStyle = SetDefaultCellStyle(workbook, false);
            ISheet sheet = workbook.CreateSheet(sheetName);

            SetSheetHeader(sheet, headerCellStyle, includeProperties);
            SetDataToSheet(sheet, cellStyle, outputs, includeProperties);

            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream, true);

            return memoryStream;
        }
        finally
        {
            if (workbook != null)
                workbook.Dispose();
        }
    }

    public byte[] ExportToSpreadsheet(IEnumerable<TOutput> outputs, ExportTypeOptions exportTypeOptions, string sheetName, string[] includeProperties)
    {
        MemoryStream memoryStream = null;
        try
        {
            memoryStream = CreateExportStream(outputs, exportTypeOptions, sheetName, includeProperties);
            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }
}
