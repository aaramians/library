using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Diagnostics
{
    public class TraceMethods
    {
        public static void TraceEnvironment()
        {
            Trace.TraceInformation("Local time {0}", DateTime.Now);
            Trace.TraceInformation("UTC time {0}", DateTime.UtcNow);
            Trace.TraceInformation("Environment UserName {0}", Environment.UserName);
            Trace.TraceInformation("Environment UserDomainName {0}", Environment.UserDomainName);
            Trace.TraceInformation("Environment UserInteractive {0}", Environment.UserInteractive);
            Trace.TraceInformation("Environment Machine {0}", Environment.MachineName);
            Trace.TraceInformation("Environment OS {0}", Environment.OSVersion);
            Trace.TraceInformation("Environment CWD {0}", Environment.CurrentDirectory);
            Trace.TraceInformation("Environment CMD {0}", Environment.CommandLine);
        }

        public static void TraceSystem()
        {
            var entry = System.Reflection.Assembly.GetEntryAssembly();
            Trace.TraceInformation("Entry Assembly {0}", entry.FullName);
        }

    }
}
