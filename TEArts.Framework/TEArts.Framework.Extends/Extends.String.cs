using System;
using System.Collections.Generic;
using System.Text;

namespace TEArts.Framework.Extends
{
    public static class StringExtends
    {
        //public static string Repeat(this char value, int count, char spliter = '\0')
        //{
        //    return Repeat(value.ToString(), count, spliter == '\0' ? string.Empty : spliter.ToString());
        //}
        public static string Repeat(this char value, int count, string spliter = "")
        {
            return Repeat(value.ToString(), count, spliter);
        }
        //public static string Repeat(this string value, int count, char spliter = '\0')
        //{
        //    return Repeat(value, count, spliter == '\0' ? string.Empty : spliter.ToString());
        //}
        public static string Repeat(this string value, int count, string spliter = "")
        {
            return RepeatInternal(value, count, spliter);
        }
        private static string RepeatInternal(string value, int count, string spliter = "")
        {
            if (count <= 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            int k = 0;
            while (k < count)
            {
                builder.Append(value);
                if (!string.IsNullOrWhiteSpace(spliter)) { builder.Append(spliter); }
                k++;
            }
            return builder.ToString();
        }
    }
}
