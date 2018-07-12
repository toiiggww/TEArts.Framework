using System;
using System.Collections.Generic;
using System.Text;

namespace TEArts.Framework.Extends
{
    public static class ByteExtends
    {
        public static bool GetBit(this byte value, byte offset)
        {
            if (offset > 8 || offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            return (value & (1 << offset)) >> offset == 1;
        }

        public static bool GetBit(this byte[] value, int index, byte offset)
        {
            if (index > value.Length || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return value[index].GetBit(offset);
        }

        public static void Append(this byte[] array, params byte[] value)
        {
            if (value == null || value.Length == 0)
            {
                return;
            }
            else
            {
                Array.Resize(ref array, (array == null || array.Length == 0) ? value.Length : array.Length + value.Length);
                Buffer.BlockCopy(value, 0, array, array.Length, value.Length);
            }
        }

        public static void Insert(this byte[] array, int index, bool isCopyOld = true, params byte[] value)
        {
            if (index <= 0 || index > array.Length || value == null || value.Length == 0)
            {
                return;
            }
            else
            {
                byte[] temp = null;
                if (isCopyOld)
                {
                    temp = new byte[array.Length - index];
                    Buffer.BlockCopy(array, index, temp, 0, temp.Length);
                }
                Array.Resize(ref array, (array == null || array.Length == 0) ? value.Length : array.Length + value.Length);
                Buffer.BlockCopy(value, 0, array, index, value.Length);
                if (isCopyOld)
                {
                    Buffer.BlockCopy(temp, 0, array, index + value.Length, temp.Length);
                    temp = null;
                }
                //GC.Collect();
            }
        }

        public static byte[] GetBytes(this bool value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this char value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this double value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this float value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this int value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this long value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this short value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this uint value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this ulong value) { return BitConverter.GetBytes(value); }
        public static byte[] GetBytes(this ushort value) { return BitConverter.GetBytes(value); }
        public static byte[] GetASCIIBytes(this string value) { return Encoding.ASCII.GetBytes(value); }
        public static byte[] GetUTF8Bytes(this string value) { return Encoding.UTF8.GetBytes(value); }

        public static string GetASCIIString(this byte[] buffer, int index, int count) { return buffer.GetString(Encoding.ASCII, index, count); }
        public static string GetUTF8String(this byte[] buffer, int index, int count) { return buffer.GetString(Encoding.UTF8, index, count); }
        public static string GetString(this byte[] buffer, Encoding encoding, int index, int count) { return encoding.GetString(buffer, index, count); }
        private static readonly List<byte> ForRepleace = new List<byte>()
        {
            0,0x08,0x09,0x0a,0x0b,0x0d,0x7f
        };
        public static string ToArrayMatrix(this byte[] value, int width = 16)
        {
            if (value == null)
            {
                return string.Empty;
            }
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            //string r = string.Empty, b = string.Empty, s = string.Empty;
            StringBuilder sb = new StringBuilder();
            #region Head Title
            int i = 0, j = 0;
            if (width > 16)
            {
                sb.Append(" ".Repeat(16));
                for (i = 0; i < width; i++)
                {
                    if (width < 100)
                    {
                        sb.AppendFormat(" {0:d2}", i);
                    }
                    else
                    {
                        sb.AppendFormat("{0:d3}", i);
                    }
                }
            }
            sb.AppendFormat("{0}Index \\ Offset  ", Environment.NewLine);
            for (i = 0; i < width; i++)
            {
                sb.AppendFormat("_{0:X2}", i);
            }
            sb.Append("     [");
            if (width <= 6)
            {
                sb.Append('_'.Repeat(width));
            }
            else
            {
                sb.Append('_'.Repeat((width - 6) / 2));
                sb.Append("string");
                sb.Append('_'.Repeat(width - ((width - 6) / 2) - 6));
            }
            sb.AppendLine("]");
            #endregion
            #region Body
            //i = value.Length / width;
            for (i = 0; i < value.Length / width; i++)
            {
                sb.Append(" ");
                sb.AppendFormat("{0:X11}", i);
                sb.Append("    ");
                for (j = i * width; j < (i + 1) * width; j++)
                {
                    sb.AppendFormat(" {0:X2}", value[j]);
                }
                sb.Append("      ");
                for (j = i * width; j < (i + 1) * width; j++)
                {
                    sb.Append(ForRepleace.Contains(value[j]) ? '.' : ((char)(value[j])));
                }
                sb.AppendLine();
            }
            #endregion
            #region Tile
            //j = value.Length % width;
            i = value.Length / width;
            sb.Append(" ");
            sb.AppendFormat("{0:X11}", i > 0 ? i + 1 : i);
            sb.Append("    ");
            i *= width;
            for (j = 0; j < value.Length % width; j++)
            {
                sb.AppendFormat(" {0:X2}", value[i + j]);
            }
            for (j = 0; j < width - value.Length % width; j++)
            {
                sb.Append("   ");
            }
            sb.Append("      ");
            for (j = 0; j < value.Length % width; j++)
            {
                sb.Append(ForRepleace.Contains(value[i + j]) ? '.' : ((char)(value[i + j])));
            }
            sb.AppendLine();
            #endregion
            return sb.ToString();
        }
    }
}
