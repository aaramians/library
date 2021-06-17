using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Library.Diagnostics
{

    public class FileTraceListener : BaseTraceListener
    {

        public bool AutoArchive { get; set; } = true;

        public override string Name { get { return base.Name; } set { base.Name = value; } }

        TextWriter str;
        private string filename;
        private SourceLevels level;

        class SourceLevelFilter : TraceFilter
        {
            private SourceLevels level;

            public SourceLevelFilter(SourceLevels level)
            {
                this.level = level;
            }

            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
            {
                if (level == SourceLevels.All)
                    return true;

                if (((int)eventType & (int)level) > 0)
                    return true;

                return false;

                //if (level == SourceLevels.Off)
                //    return false;

                //if (level == SourceLevels.Critical)
                //    return eventType == TraceEventType.Critical;

                //if (level == SourceLevels.Error)
                //    return eventType == (TraceEventType.Critical | TraceEventType.Error);

                //if (level == SourceLevels.Warning)
                //    return eventType & (TraceEventType.Critical | TraceEventType.Error | TraceEventType.Warning);

                //if (level == SourceLevels.Information)
                //    return eventType == (TraceEventType.Critical | TraceEventType.Error | TraceEventType.Warning | TraceEventType.Information);

                //if (level == SourceLevels.Verbose)
                //    return eventType == (TraceEventType.Critical | TraceEventType.Error | TraceEventType.Warning | TraceEventType.Information | TraceEventType.Verbose);

                //if (level == SourceLevels.ActivityTracing)
                //    return eventType == (TraceEventType.Start | TraceEventType.Stop | TraceEventType.Transfer | TraceEventType.Suspend);

            }
        }

        public FileTraceListener(SourceLevels level) : base()
        {
            this.level = level;
            this.Filter = new SourceLevelFilter(level);
            SetupStream();
        }

        public FileTraceListener(SourceLevels level, string filename) : base()
        {
            this.level = level;
            this.Filter = new SourceLevelFilter(level);
            this.filename = filename;

            SetupStream();
        }

        private void SetupStream()
        {
            //throw new InvalidOperationException();
   
            if (str == null)
                if (this.filename != null)
                    str = new StreamWriter(this.filename, true);

            if (str == null)
            {
                var directory = new DirectoryInfo(".");

                var logfiles = directory.GetFiles($"{AppDomain.CurrentDomain.FriendlyName}.log*.txt");

                var logfile = logfiles
                    .OrderByDescending(f => f.LastWriteTime)
                    .FirstOrDefault();

                try
                {
                    if (AutoArchive)
                        if (logfile != null)
                            if (logfile.Length > 50 * 1024 * 1024)
                            {
                                // todo check if 7z doesnt exists extract it from project resource

                                var nlogfile = new FileInfo($"{logfile.FullName}.{DateTime.Now.ToString("yyyyMMddTHHmmss")}");

                                File.Move(logfile.FullName, nlogfile.FullName);
                                logfile = null;

                                var CurrentDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Directory;
                                var z7zLoc = CurrentDirectory.GetFiles("7z.exe", SearchOption.AllDirectories)
                                    .ToList()
                                    .FirstOrDefault();

                                if (z7zLoc != null)
                                {
                                    var z7z = Process.Start(new ProcessStartInfo()
                                    {
                                        FileName = z7zLoc.FullName,
                                        CreateNoWindow = false,
                                        WorkingDirectory = CurrentDirectory.FullName,
                                        UseShellExecute = false,
                                        Arguments = $"a logs.archive.7z {nlogfile.Name} -sdel"
                                    });
                                    z7z.WaitForExit();
                                }
                            }
                }
                catch (Exception)
                {

                }

                try
                {
                    str = new StreamWriter($"{AppDomain.CurrentDomain.FriendlyName}.log.txt", true);
                }
                catch (Exception)
                {

                }

                if (str == null)
                    try
                    {
                        str = new StreamWriter($"{AppDomain.CurrentDomain.FriendlyName}.log.{DateTime.Now.ToString("yyyyMMddTHHmmss")}.txt", true);
                    }
                    catch (Exception)
                    {

                    }
            }
        }



        public override void Write(string message)
        {
            str.Write(message);
        }

        public override void WriteLine(string message)
        {
            if ((this.TraceOutputOptions & TraceOptions.DateTime) > 0)
            {
                str.Write(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
                str.Write(" ");
            }

            str.Write(strUnknown);
            str.Write(" ");
            str.WriteLine(message);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                return;

            if ((this.TraceOutputOptions & TraceOptions.DateTime) > 0)
            {

                str.Write(eventCache.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
                str.Write(" ");
            }

            string strEventType = strUnknown;
            elookup.TryGetValue(eventType, out strEventType);

            str.Write(strEventType);
            str.Write(" ");
            str.WriteLine(message);
        }


        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                return;

            if ((this.TraceOutputOptions & TraceOptions.DateTime) > 0)
            {
                str.Write(eventCache.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
                str.Write(" ");
            }


            string strEventType = strUnknown;
            elookup.TryGetValue(eventType, out strEventType);

            str.Write(strEventType);
            str.Write(" ");
            str.WriteLine(format, args);
        }

        public override void Flush()
        {
            base.Flush();
            str.Flush();
        }

        public override void Close()
        {
            str.Close();
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            str.Dispose();
            base.Dispose(disposing);
        }
    }

}
