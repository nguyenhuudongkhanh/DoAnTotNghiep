using ExcelDataReader;
using System.Data;
using System.Reflection;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

public class Converter
{
    private object To(object input, Type outputType)
    {
        object output = Activator.CreateInstance(outputType);
        Type inputType = input.GetType();
        PropertyInfo[] properties = output.GetType().GetProperties();

        foreach (PropertyInfo property in properties)
        {
            PropertyInfo inputProperty = inputType.GetProperty(property.Name);
            if (inputProperty != null)
            {
                property.SetValue(output, inputProperty.GetValue(input));
            }
        }

        return output;
    }

    public TOutput To<TInput, TOutput>(TInput input)
    {
        return (TOutput)To(input, typeof(TOutput));
    }

    public IEnumerable<TOutput> To<TInput, TOutput>(IEnumerable<TInput> inputs, Func<TInput, TOutput> predicate)
    {
        foreach (TInput input in inputs)
        {
            yield return predicate(input);
        }
    }

    public IEnumerable<TModel> To<TModel>(IExcelDataReader excelDataReader, int startFromRow, Func<IExcelDataReader, TModel> predicate)
    {
        int rowIndex = 0;
        while (excelDataReader.Read())
        {
            if (rowIndex >= startFromRow)
            {
                yield return predicate(excelDataReader);
            }
            rowIndex++;
        }
    }

    public IEnumerable<TModel> To<TModel>(DataTable dataTable, int startFromRow, Func<DataRow, TModel> predicate)
    {
        for (int rowIndex = startFromRow; rowIndex < dataTable.Rows.Count; rowIndex++)
        {
            yield return predicate(dataTable.Rows[rowIndex]);
        }
    }
}