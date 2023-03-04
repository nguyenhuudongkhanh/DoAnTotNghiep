using ExcelDataReader;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

internal class ImportHelper<TEntity> where TEntity : class
{
    private DbContext _context;
    private DbSet<TEntity> _dbSet;
    private Converter _converter;

    public ImportHelper(DbContext context, DbSet<TEntity> dbSet, Converter converter)
    {
        _context = context;
        _dbSet = dbSet;
        _converter = converter;
    }

    public DataResponse ImportFromSpreadsheet(Stream stream, int startFromRow, Func<IExcelDataReader, TEntity> predicate)
    {
        IExcelDataReader excelDataReader = null;
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            excelDataReader = ExcelReaderFactory.CreateReader(stream);
            _dbSet.BulkInsert(_converter.To(excelDataReader, startFromRow, predicate));
            _context.BulkSaveChanges();

            return new DataResponse { Status = DataResponseStatus.Success };
        }
        finally
        {
            if (excelDataReader != null)
                excelDataReader.Dispose();
        }
    }

    public DataResponse ImportFromSpreadsheet(Stream stream, int startFromRow, string sheetName, Func<DataRow, TEntity> predicate)
    {
        IExcelDataReader excelDataReader = null;
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            excelDataReader = ExcelReaderFactory.CreateReader(stream);

            DataSet dataSet = excelDataReader.AsDataSet();
            DataTable dataTable = null;
            if (string.IsNullOrEmpty(sheetName))
                dataTable = dataSet.Tables[0];
            else
                dataTable = dataSet.Tables[sheetName];

            if (dataTable == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            _dbSet.BulkInsert(_converter.To<TEntity>(dataTable, startFromRow, predicate));
            _context.BulkSaveChanges();

            return new DataResponse { Status = DataResponseStatus.Success };
        }
        finally
        {
            if (excelDataReader != null)
                excelDataReader.Dispose();
        }
    }

    public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, int startFromRow, Func<IExcelDataReader, TEntity> predicate)
    {
        IExcelDataReader excelDataReader = null;
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            excelDataReader = ExcelReaderFactory.CreateReader(stream);
            await _dbSet.BulkInsertAsync(_converter.To(excelDataReader, startFromRow, predicate));
            await _context.BulkSaveChangesAsync();

            return new DataResponse { Status = DataResponseStatus.Success };
        }
        finally
        {
            if (excelDataReader != null)
                excelDataReader.Dispose();
        }
    }

    public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, int startFromRow, string sheetName, Func<DataRow, TEntity> predicate)
    {
        IExcelDataReader excelDataReader = null;
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            excelDataReader = ExcelReaderFactory.CreateReader(stream);

            DataSet dataSet = excelDataReader.AsDataSet();
            DataTable dataTable = null;
            if (string.IsNullOrEmpty(sheetName))
                dataTable = dataSet.Tables[0];
            else
                dataTable = dataSet.Tables[sheetName];

            if (dataTable == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            await _dbSet.BulkInsertAsync(_converter.To(dataTable, startFromRow, predicate));
            await _context.BulkSaveChangesAsync();

            return new DataResponse { Status = DataResponseStatus.Success };
        }
        finally
        {
            if (excelDataReader != null)
                excelDataReader.Dispose();
        }
    }
}
