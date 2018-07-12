using System;
using System.Collections.Generic;
using System.Text;

namespace TEArts.Framework.Extends
{
    public static class ArrayExtends
    {
        public static string Concate(this Array ary, string spliter = ", ")
        {
            string ret = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var i in ary)
            {
                sb.AppendFormat("{0}{1}", i, spliter);
            }
            ret = sb.ToString().TrimEnd(spliter.ToCharArray());
            return ret;
        }

    }
}
