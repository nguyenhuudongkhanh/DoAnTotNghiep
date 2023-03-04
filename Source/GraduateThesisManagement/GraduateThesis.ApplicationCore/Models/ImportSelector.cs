using ExcelDataReader;
using System.Data;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class ImportSelector<TEntity> where TEntity : class
{
    public Func<IExcelDataReader, TEntity> SimpleImportSpreadsheet { get; set; }
    public Func<DataRow, TEntity> AdvancedImportSpreadsheet { get; set; }
}
