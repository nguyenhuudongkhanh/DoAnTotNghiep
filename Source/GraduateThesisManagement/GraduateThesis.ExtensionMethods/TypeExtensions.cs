using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GraduateThesis.ExtensionMethods;

public static class TypeExtensions
{
    /// <summary>
    /// This Methode extends the System.Type-type to get all extended methods. It searches hereby in all assemblies which are known by the current AppDomain.
    /// </summary>
    /// <remarks>
    /// Insired by Jon Skeet from his answer on http://stackoverflow.com/questions/299515/c-sharp-reflection-to-identify-extension-methods
    /// </remarks>
    /// <returns>returns MethodInfo[] with the extended Method</returns>

    public static MethodInfo[] GetExtensionMethods(this Type t)
    {
        List<Type> assTypes = new List<Type>();

        foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
        {
            assTypes.AddRange(item.GetTypes());
        }

        assTypes = assTypes.Where(x => x.Name.Contains("SaveChanges")).ToList();

        var query = from type in assTypes
                    where type.IsSealed && !type.IsGenericType && !type.IsNested
                    from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    where method.IsDefined(typeof(ExtensionAttribute), false)
                    where method.GetParameters()[0].ParameterType == t
                    select method;
        return query.ToArray<MethodInfo>();
    }

    /// <summary>
    /// Extends the System.Type-type to search for a given extended MethodeName.
    /// </summary>
    /// <param name="MethodeName">Name of the Methode</param>
    /// <returns>the found Methode or null</returns>
    public static MethodInfo GetExtensionMethod(this Type t, string methodeName)
    {
        var mi = from methode in t.GetExtensionMethods()
                 where methode.Name == methodeName
                 select methode;
        if (mi.Count<MethodInfo>() <= 0)
            return null;
        else
            return mi.First<MethodInfo>();
    }
}
