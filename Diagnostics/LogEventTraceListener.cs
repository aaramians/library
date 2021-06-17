using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Diagnostics
{
    public class LogEvent
    {
        public DateTime Date;
        public long? Timestamp;
        public Guid Session;
        public int? UserID;
        public string Source;
        public string Data;
    }

    public abstract class LogEventTraceListener : System.Diagnostics.TraceListener
    {
        public ConcurrentQueue<LogEvent> LogEventQueue;

        public abstract Guid Session { get; }
        public abstract int? UserID { get; }

        public LogEventTraceListener()
            : base()
        {
            LogEventQueue = new ConcurrentQueue<LogEvent>();
        }

        public override void Write(string message)
        {
            LogEventQueue.Enqueue(new LogEvent()
            {
                Date = DateTime.UtcNow,
                Timestamp = Stopwatch.GetTimestamp(),
                Session = Session,
                UserID = UserID,
                Source = "Trace.Write.Message",
                Data = message,
            });
        }

        public override void Write(string message, string category)
        {
            LogEventQueue.Enqueue(new LogEvent()
            {
                Date = DateTime.UtcNow,
                Timestamp = Stopwatch.GetTimestamp(),
                Session = Session,
                UserID = UserID,
                Source = "Trace.Write." + category,
                Data = message,
            });
        }

        public override void WriteLine(string message)
        {
            LogEventQueue.Enqueue(new LogEvent()
            {
                Date = DateTime.UtcNow,
                Timestamp = Stopwatch.GetTimestamp(),
                Session = Session,
                UserID = UserID,
                Source = "Trace.Write",
                Data = message,
            });
        }
        public override void WriteLine(string message, string category)
        {
            LogEventQueue.Enqueue(new LogEvent()
            {
                Date = DateTime.UtcNow,
                Timestamp = Stopwatch.GetTimestamp(),
                Session = Session,
                UserID = UserID,
                Source = "Trace.WriteLine." + category,
                Data = message,
            });
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, string.Empty, null, null, null))
                return;

            LogEventQueue.Enqueue(new LogEvent()
            {
                Date = eventCache.DateTime.ToUniversalTime(),
                Timestamp = eventCache.Timestamp,
                Session = Session,
                UserID = UserID,
                Source = string.Format("Trace.{0}", eventType),
                Data = message,
            });
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
                return;

            if (args == null)
                LogEventQueue.Enqueue(new LogEvent()
                {
                    Date = eventCache.DateTime.ToUniversalTime(),
                    Timestamp = eventCache.Timestamp,
                    Session = Session,
                    UserID = UserID,
                    Source = string.Format("Trace.{0}", eventType),
                    Data = format
                });
            else
                LogEventQueue.Enqueue(new LogEvent()
                {
                    Date = eventCache.DateTime.ToUniversalTime(),
                    Timestamp = eventCache.Timestamp,
                    Session = Session,
                    UserID = UserID,
                    Source = string.Format("Trace.{0}", eventType),
                    Data = string.Format(format, args)
                });
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
