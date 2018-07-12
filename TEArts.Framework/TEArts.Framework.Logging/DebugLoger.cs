using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;
using TEArts.Framework.Extends;

namespace TEArts.Framework.Logging
{
    public interface IDebugerLoger
    {
        DebugType DebugType { get; set; }
        void WriteLog(object log, DebugType type);
        void RegistLoger();
    }
    public enum DebugType { Error, AuditSuc, AuditFal, Warning, Info, Debug, FunCall }
    public abstract class DebugerLoger : MarshalByRefObject, IDebugerLoger
    {
        public DebugerLoger()
        {
            DebugType = DebugType.Debug;
        }
        public virtual DebugType DebugType { get; set; }
        public virtual void WriteLog(object log, DebugType type) { }
        public virtual void RegistLoger() { }
        public static int ObjectDeeps { get; set; } = 3;
        public static string BuildLog(object o, int level = 0)
        {
            if (o == null)
            {
                return "[NulL]";
            }
            if (level > ObjectDeeps)
            {
                return o.ToString();
            }
            if (o.IsBaseType())
            {
                return o.ToString();
            }
            StringBuilder sb = new StringBuilder();
            level++;
            Type t = o.GetType();
            PropertyInfo[] ps = t.GetProperties();
            sb.AppendFormat("{0}Type : {0}", "  ".Repeat(level), t.FullName);
            string generic = string.Empty;
            foreach (PropertyInfo p in ps)
            {
                try
                {
                    sb.AppendFormat(
                        "{0}{1} {2} {3}{4}",
                        "  ".Repeat(level),
                        p.DeclaringType.GenericDeclare(),
                        p.Name,
                        p.PropertyType.IsBaseType() ? "\t : " + p.GetValue(o, null) : Environment.NewLine + BuildLog(p.GetValue(o, null), level + 1),
                        Environment.NewLine
                    );
                }
                catch { }
            }
            return sb.ToString();
        }
    }
    public class ConsoleLoger : DebugerLoger
    {
        private ConsoleLoger()
        {

        }
        private static readonly object mbrOutLocker = new object();
        private static ConsoleLoger mbrInstance;
        public static ConsoleLoger Instance
        {
            get
            {
                if (mbrInstance == null)
                {
                    mbrInstance = new ConsoleLoger();
                }
                return mbrInstance;
            }
        }
        private ConsoleColor Back { get; set; }
        private ConsoleColor Fore { get; set; }
        public override void WriteLog(object log, DebugType type)
        {
            lock (mbrOutLocker)
            {
                switch (type)
                {
                    case DebugType.Error:
                        Back = ConsoleColor.Red;
                        Fore = ConsoleColor.White;
                        break;
                    case DebugType.Warning:
                        Back = ConsoleColor.Yellow;
                        Fore = ConsoleColor.Red;
                        break;
                    case DebugType.AuditSuc:
                        Back = ConsoleColor.DarkGray;
                        Fore = ConsoleColor.White;
                        break;
                    case DebugType.AuditFal:
                        Back = ConsoleColor.DarkRed;
                        Fore = ConsoleColor.DarkYellow;
                        break;
                    case DebugType.Info:
                        Back = ConsoleColor.Black;
                        Fore = ConsoleColor.White;
                        break;
                    case DebugType.Debug:
                        Back = ConsoleColor.Gray;
                        Fore = ConsoleColor.DarkGreen;
                        break;
                    case DebugType.FunCall:
                        Back = ConsoleColor.Gray;
                        Fore = ConsoleColor.Yellow;
                        break;
                    default:
                        break;
                }
                Console.BackgroundColor = Back;
                Console.ForegroundColor = Fore;
                string[] s = BuildLog(log).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //foreach (string l in s)
                //{
                //    if (type <= DebugType)
                //    {
                //        Console.WriteLine(string.Format("{0}\t{1}\t{2}", DateTime.Now, type, l));
                //    }
                //}
                for (int i = 0; i < s.Length - 1; i++)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", DateTime.Now, type, s[i]);
                }
                if (s.Length > 0)
                {
                    Console.Write("{0}\t{1}\t{2}", DateTime.Now, type, s[s.Length - 1]);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }
        public override void RegistLoger()
        {
            Debuger.Loger.AddLoger("Console", this);
        }
    }
    //public class EventsLoger : DebugerLoger
    //{
    //    private static Dictionary<string, EventLog> mbrEvents;
    //    private EventLog mbrLoger;
    //    public EventsLoger(string source, string name)
    //    {
    //        if (mbrEvents == null)
    //        {
    //            mbrEvents = new Dictionary<string, EventLog>();
    //        }
    //        if (!mbrEvents.ContainsKey(name))
    //        {
    //            if (!EventLog.SourceExists(name))
    //            {
    //                EventLog.CreateEventSource(source, name);
    //            }
    //            mbrLoger = new EventLog();
    //            mbrLoger.Source = source;
    //            mbrLoger.Log = name;
    //            mbrEvents.Add(source, mbrLoger);
    //        }
    //        else
    //        {
    //            mbrLoger = mbrEvents[source];
    //        }
    //    }
    //    public override void WriteLog(object log, DebugType type)
    //    {
    //        if (type <= DebugType)
    //        {
    //            mbrLoger.WriteEntry(string.Format("{0}{1}{2}", DateTime.Now, Environment.NewLine, BuildLog(log)));
    //        }
    //    }
    //    public override void RegistLoger()
    //    {
    //        Debuger.Loger.AddLoger("Events", this);
    //    }
    //}
    public class NetworkLoger : DebugerLoger
    {
        private System.Net.Sockets.Socket mbrSocket;
        public NetworkLoger(System.Net.IPAddress ip, int port, System.Net.Sockets.ProtocolType type)
        {
        }
        public override void WriteLog(object log, DebugType type)
        {
            if (type <= DebugType)
            {
                mbrSocket.Send(Encoding.UTF8.GetBytes(string.Format("{0}{1}{2}", DateTime.Now, Environment.NewLine, BuildLog(log))));
            }
        }
        public override void RegistLoger()
        {
            Debuger.Loger.AddLoger("Network", this);
        }
    }
    public class TextFileLoger : DebugerLoger
    {
        public TextFileLoger()
            : this("Application.log")
        {
        }
        public TextFileLoger(string file)
        {
            FileName = file;
            Writer = new StreamWriter(FileName, true, Encoding.UTF8);
            Writer.WriteLine(string.Format("{0}\t{1}\t{2}", DateTime.Now, DebugType.Info, "Logger ready for writting."));
            Writer.Flush();
        }
        public string FileName { get; set; }
        private TextWriter Writer { get; set; }
        public override void WriteLog(object log, DebugType type)
        {
            if (type <= DebugType)
            {
                Writer.WriteLine(string.Format("{0}\t{1}\t{2}", DateTime.Now, type, BuildLog(log)));
                Writer.Flush();
            }
        }
        public override void RegistLoger()
        {
            Debuger.Loger.AddLoger("TextLoger", this);
        }
    }
    public class Debuger
    {
        private static object mbrDebugHandler = new object();
        private static Debuger mbrInstance;
        private Dictionary<string, IDebugerLoger> mbrLogers;
        private Debuger()
        {
            mbrLogers = new Dictionary<string, IDebugerLoger>();
        }
        public void AddLoger(string key, IDebugerLoger loger)
        {
            if (!mbrLogers.ContainsKey(key))
            {
                mbrLogers.Add(key, loger);
            }
        }
        public static Debuger Loger
        {
            get
            {
                if (mbrInstance == null)
                {
                    mbrInstance = new Debuger();
                }
                return mbrInstance;
            }
        }

        public bool ViewStackTrace { get; set; }

        private void DebugInfoInternal(object o, DebugType type)
        {
            //Console.WriteLine((new System.Diagnostics.StackTrace()).ToString());
            lock (mbrDebugHandler)
            {
                if (o == null)
                {
                    DebugInfoInternal("====]> NULL <[====", type);
                }
                else
                {
                    if (mbrLogers.Count == 0)
                    {
                        ConsoleLoger.Instance.RegistLoger();
                    }
                    if (ViewStackTrace)
                    {
                        if (type == DebugType.Error || type == DebugType.FunCall)
                        {
                            string[] stk = new StackTrace(2, true).ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (IDebugerLoger l in mbrLogers.Values)
                            {
                                foreach (string str in stk)
                                {
                                    l.WriteLog(str, type);
                                }
                            }
                        }
                        else if (type == DebugType.Warning)
                        {
                            string[] stk = new StackTrace(2, false).ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (IDebugerLoger l in mbrLogers.Values)
                            {
                                foreach (string str in stk)
                                {
                                    l.WriteLog(str, type);
                                }
                            }
                        }
                    }

                    foreach (IDebugerLoger l in mbrLogers.Values)
                    {
                        l.WriteLog(o, type);
                    }
                }
            }
        }
        public void DebugInfo(DebugType type, byte[] message)
        {
            DebugInfoInternal(message.ToArrayMatrix() as object, type);
        }
        public void DebugInfo(DebugType type, string message)
        {
            DebugInfoInternal(message, type);
        }
        public void Info(string formater, params object[] args)
        {
            DebugInfo(DebugType.Info, formater == null ? string.Empty : formater, args);
        }
        public void DebugInfo(DebugType type, string formater, params object[] args)
        {
            try
            {
                DebugInfoInternal(string.Format(formater, args), type);
            }
            catch
            {
                DebugInfoInternal(formater.Replace("{", "{{").Replace("}", "}}"), type);
                foreach (object o in args)
                {
                    DebugInfoInternal(o, type);
                }
            }
        }
        public void Info(object o) { DebugInfoInternal(o, DebugType.Info); }
        public void Error(string formater, params object[] args)
        {
            DebugInfo(DebugType.Error, formater, args);
        }
        public void Warning(string formater, params object[] args)
        {
            DebugInfo(DebugType.Warning, formater, args);
        }

        public void DebugInfo(string info)
        {
            DebugInfo(DebugType.Info, info);
        }
    }
}
