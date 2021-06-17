using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Diagnostics
{
    public class BaseTraceListener : TraceListener
    {
        enum Flash { PeriodicFlush, AutoFlush, OnCloseFlush }

        public static Dictionary<TraceEventType, string> elookup = new Dictionary<TraceEventType, string>();

        internal static string strUnknown = "[UNKN]";

        static BaseTraceListener()
        {
            lock (elookup)
            {
                elookup.Add(TraceEventType.Critical, "[CRIT]");
                elookup.Add(TraceEventType.Error, "[ERRO]");
                elookup.Add(TraceEventType.Information, "[INFO]");
                elookup.Add(TraceEventType.Resume, "[RESU]");
                elookup.Add(TraceEventType.Start, "[STAR]");
                elookup.Add(TraceEventType.Stop, "[STOP]");
                elookup.Add(TraceEventType.Suspend, "[SUSP]");
                elookup.Add(TraceEventType.Transfer, "[TRAN]");
                elookup.Add(TraceEventType.Verbose, "[VERB]");
                elookup.Add(TraceEventType.Warning, "[WARN]");
            }
        }

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }

}
