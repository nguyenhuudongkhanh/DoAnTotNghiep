using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ExtensionMethods;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

public class ExpressionBuilder
{
    public string GetWhereExpString(string prefix)
    {
        return $"{prefix} => {prefix}.IsDeleted == false";
    }

    public string GetWhereExpString(string prefix, string propertyName, object value)
    {
        string valueInExpression = null;
        if (value.IsString())
            valueInExpression = $"\"{value}\"";

        if (value.IsNumber() || value.IsBool())
            valueInExpression = value.ToString()!;

        if (string.IsNullOrEmpty(valueInExpression))
            return null!;

        return $"{prefix} => ({prefix}.{propertyName} == {valueInExpression}) && {prefix}.IsDeleted == false";
    }

    public string GetWhereExpString<TEntity>(string prefix, string[] conditions, string keyword)
    {
        StringBuilder expStringBuilder = new StringBuilder($"{prefix} => ");
        PropertyInfo[] properties = typeof(TEntity).GetProperties()
            .Where(p => p.PropertyType == typeof(string)).ToArray();

        if (properties.Length == 0)
        {
            expStringBuilder.Append($"{prefix}.IsDeleted == false");
            return expStringBuilder.ToString();
        }

        expStringBuilder.Append("(");
        int count = 1;
        foreach (PropertyInfo property in properties)
        {
            if (count == properties.Length)
                expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\")");
            else
                expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\") || ");

            count++;
        }

        expStringBuilder.Append(") && ");
        foreach (string condition in conditions)
        {
            expStringBuilder.Append($"{prefix}.{condition} && ");
        }

        expStringBuilder.Append($"{prefix}.IsDeleted == false");

        return expStringBuilder.ToString();
    }

    public string GetWhereExpString<TEntity>(string prefix, string keyword)
    {
        StringBuilder expStringBuilder = new StringBuilder($"{prefix} => ");
        PropertyInfo[] properties = typeof(TEntity).GetProperties()
            .Where(p => p.PropertyType == typeof(string)).ToArray();

        if (properties.Length == 0)
        {
            expStringBuilder.Append($"{prefix}.IsDeleted == false");
            return expStringBuilder.ToString();
        }

        expStringBuilder.Append("(");
        int count = 1;
        foreach (PropertyInfo property in properties)
        {
            if (count == properties.Length)
                expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\")");
            else
                expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\") || ");

            count++;
        }

        expStringBuilder.Append($") && {prefix}.IsDeleted == false");

        return expStringBuilder.ToString();
    }

    public string GetWhereExpString<TEntity>(string prefix, string searchBy, object value)
    {
        StringBuilder expStringBuilder = new StringBuilder($"{prefix} => ");
        PropertyInfo property = typeof(TEntity).GetProperty(searchBy);
        if (property == null)
            throw new Exception($"Property named '{searchBy}' not found");

        if (property.PropertyType == typeof(string))
        {
            expStringBuilder.Append($"{prefix}{searchBy}.Contains(\"{value}\")");
        }
        else if (property.PropertyType == typeof(bool))
        {
            string boolValueAsString = ((bool)value) ? "true" : "false";
            expStringBuilder.Append($"{prefix}{searchBy} == {boolValueAsString}");
        }
        else if (
            property.PropertyType == typeof(int) || property.PropertyType == typeof(long)
            || property.PropertyType == typeof(float) || property.PropertyType == typeof(double)
        )
        {
            expStringBuilder.Append($"{prefix}{searchBy} == {value}");
        }
        else
        {
            throw new Exception($"This data type is not supported!");
        }

        expStringBuilder.Append($"&& {prefix}.IsDeleted == false");

        return expStringBuilder.ToString();
    }

    public string GetOrderByExpString<TEntity>(string prefix, string propertyName)
    {
        if (typeof(TEntity).GetProperty(propertyName) == null)
            throw new Exception($"Property named '{propertyName}' not found");

        return $"{prefix} => {prefix}.{propertyName}";
    }

    public Expression<Func<TEntity, object>> GetOrderByExpression<TEntity>(string prefix, string propertyName)
    {
        string orderByExpString = GetOrderByExpString<TEntity>(prefix, propertyName);
        Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser
            .ParseLambda<TEntity, object>(new ParsingConfig(), true, orderByExpString);

        return orderExpression;
    }

    public string BuildExpString(RecordFilter recordFilter)
    {

        return null;
    }
}
