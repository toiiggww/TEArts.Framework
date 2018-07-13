using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
namespace TEArts.Framework.Logging
{
    /// <summary>
    /// Log4NetObject
    /// </summary>
    public static class Log4NetObject
    {
        public const string InfoLogger = "InfoLogger",
            ErrorLogger = "ErrorLogger",
            FatalLogger = "FatalLogger",
            WarnLogger = "WarnLogger",
            DebugLogger = "DebugLogger";

        public static bool NotInited { get; private set; } = true;

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Log4NetInit()
        {
            try
            {
                Log4NetInit(Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="directory"></param>
        public static void Log4NetInit(string directory)
        {
            try
            {
                if (NotInited)
                {
                    var path = Path.GetDirectoryName(directory) + "\\" + "log4net.config";
                    if (File.Exists(path))
                    {
                        XmlConfigurator.ConfigureAndWatch((new FileInfo(Path.GetDirectoryName(directory) + "\\" + "log4net.config")));
                    }
                    NotInited = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static ILog Log(string loggerName)
        {
            return LogManager.GetLogger(loggerName);
        }

        /// <summary>
        /// 信息日志记录
        /// </summary>
        /// <param name="strLog">日志内容</param>
        /// <param name="loggerName">Logger配置名称</param>
        /// <param name="exp">异常信息</param>
        public static void WriteLog(string strLog, string loggerName = "InfoLogger", Exception exp = null)
        {
            try
            {
                if (exp == null && Log(loggerName).IsInfoEnabled)
                {
                    Log(loggerName).Info(strLog);
                }
                else
                {
                    Log(loggerName).Info(strLog, exp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion
    }
}
