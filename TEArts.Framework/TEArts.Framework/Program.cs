using System;
using System.Collections.Generic;
using TEArts.Framework.Config;
using TEArts.Framework.Extends;
using TEArts.Framework.Logging;

namespace TEArts.Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            DebugLog.Loger.AddLoger(nameof(ConsoleLoger), ConsoleLoger.Instance);
            DebugLog.Loger.Warning("xxxxxxxxxxxxxxxxxxxxx".Repeat(10));
            int k = AppConfigService.GetConfig<int>("tk", x => Convert.ToInt32(x), x => x.ToString());
            List<byte> bs = new List<byte>();
            for (byte i = byte.MinValue; i < byte.MaxValue; i++)
            {
                bs.Add(i);
            }
            DebugLog.Loger.Info(bs.ToArray().ToArrayMatrix());
            DebugLog.Loger.Error(bs.ToArray().ToArrayMatrix(18));
            DebugLog.Loger.Error(bs.ToArray().ToArrayMatrix(32));
            DebugLog.Loger.Error(bs.ToArray().ToArrayMatrix(58));
            DebugLog.Loger.Error(bs.ToArray().ToArrayMatrix(48));
            Console.Read();
        }
    }
}
