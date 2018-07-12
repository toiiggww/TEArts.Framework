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
        public static string ToArrayMatrix(this byte[] value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            //string r = string.Empty, b = string.Empty, s = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}Index \\ Offset  ", Environment.NewLine);
            int i = 0, j = 0;
            for (; i < 16; i++)
            {
                sb.AppendFormat("_{0:X2}", i);
            }
            sb.AppendLine(" [_____string_____]");
            i = value.Length / 16;
            j = value.Length % 16;
            return sb.ToString();
        }

    }
}
