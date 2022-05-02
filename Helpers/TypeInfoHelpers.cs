using System.Collections;
using System.Reflection;

namespace MyORM
{
    public static class TypeInfoHelpers
    {
        public static bool IsArray(this PropertyInfo info)
        {
            return IsArray(info.PropertyType);
        }

        public static bool IsArray(this Type type)
        {
            return type.IsAssignableTo(typeof(IEnumerable)) && type != typeof(string);
        }

        public static Type? GetArrayElementType(this Type type)
        {
            if (!IsArray(type))
                return null;
            else
                return type.GetElementType() ?? type.GetGenericArguments()[0];
        }

        public static Type?[] GetArrayElementTypes(this Type type)
        {
#pragma warning disable
            if (!IsArray(type))
                return null;
#pragma warning enable
            else
               return type.GetElementType() != null ? new Type?[] { type.GetElementType() } : type.GetGenericArguments();
        }

        public static IList? CreateListFromType(this Type type)
        {
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(type)) as IList;
        }

        public static Array? CreateArrayFromType(this Type type, int length)
        {
            return Array.CreateInstance(type, length) as Array;
        }


    }
}
