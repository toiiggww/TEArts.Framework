using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TEArts.Framework.Extends
{
    public static class TypeExtends
    {
        public static readonly List<Type> BaseTypes = new List<Type>
        {
            typeof(bool), typeof(byte), typeof(char),
            typeof(DateTime), typeof(decimal), typeof(double),
            typeof(float), typeof(Guid), typeof(int),
            typeof(IntPtr), typeof(long), typeof(object),
            typeof(sbyte), typeof(short), typeof(string),
            typeof(TimeSpan), typeof(Type), typeof(uint),
            typeof(ushort), typeof(ulong), typeof(UIntPtr),
            typeof(void), typeof(FieldInfo), typeof(PropertyInfo),
            typeof(MethodInfo), typeof(Exception)
        };
        public static bool IsBaseType(this Type t) { return BaseTypes.Contains(t); }
        public static bool IsBaseType(this object o) { return o.GetType().IsBaseType(); }
        public static string GenericDeclare(this Type t)
        {
            if (t.IsGenericType)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(t.Name);
                builder.Append("<");
                foreach (Type g in t.GetGenericArguments())
                {
                    builder.Append(g.IsGenericType ? g.GenericDeclare() : g.Name);
                    builder.Append(", ");
                }
                return builder.ToString().TrimEnd(',', ' ') + ">";
            }
            else
            {
                return t.Name;
            }
        }
    }
}
