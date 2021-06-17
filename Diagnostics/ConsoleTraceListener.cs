using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Diagnostics
{

    public class ConsoleTraceListener : BaseTraceListener
    {
        private SourceLevels level;

        public ConsoleColor ConsoleForegroundColor { get; }

        public ConsoleTraceListener() : base()
        {
            this.ConsoleForegroundColor = Console.ForegroundColor;
        }
        public ConsoleTraceListener(SourceLevels level) : this()
        {
            this.level = level;
        }

        public override void Write(string message)
        {
            Console.Write(message);

        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                return;

            switch (eventType)
            {
                case TraceEventType.Critical:
                    break;
                case TraceEventType.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case TraceEventType.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case TraceEventType.Information:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Verbose:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Start:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Stop:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Suspend:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Resume:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Transfer:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                default:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
            }

            if ((this.TraceOutputOptions & TraceOptions.DateTime) > 0)
            {
                Console.Write(eventCache.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Console.Write(" ");
            }

            string strEventType = strUnknown;
            elookup.TryGetValue(eventType, out strEventType);

            Console.Write(strEventType);
            Console.Write(" ");
            Console.WriteLine(message);

            Console.ForegroundColor = ConsoleForegroundColor;
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                return;

            switch (eventType)
            {
                case TraceEventType.Critical:
                    break;
                case TraceEventType.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case TraceEventType.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case TraceEventType.Information:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Verbose:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Start:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Stop:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Suspend:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Resume:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                case TraceEventType.Transfer:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
                default:
                    Console.ForegroundColor = ConsoleForegroundColor;
                    break;
            }

            if ((this.TraceOutputOptions & TraceOptions.DateTime) > 0)
            {
                Console.Write(eventCache.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Console.Write(" ");
            }


            string strEventType = strUnknown;
            elookup.TryGetValue(eventType, out strEventType);


            Console.Write(strEventType);
            Console.Write(" ");
            Console.WriteLine(format, args);

            Console.ForegroundColor = ConsoleForegroundColor;
        }
    }
}
