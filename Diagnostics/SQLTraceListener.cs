using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Diagnostics
{
    public abstract class SQLTraceListener : LogEventTraceListener
    {
        const string SQLDML_Trace = "SELECT ID, Date, TimeStamp, Session, UserID,ProcessID, Source, Severity,Data,Tag FROM KST_EZC_Trace WHERE 1 = 0";
        const string SQLDML_TraceTable = "KST_EZC_Trace";
        const int SQLCNF_UpdateTimeout = 60;

        Thread Servicethread;
        DateTime Lasttick;

        bool Keep;
        ManualResetEvent Watchdog;

        DataTable schemadt;
        SqlCommand updatecmd;

        int interval;

        public virtual SqlConnection GetConnection()
        {
            return null;// ConnectionManager.ConnectionStringBuilder("").ToString();
        }

        public SQLTraceListener() : base() { }

        public void AutoFlush(int interval)
        {
            if (Servicethread != null)
                throw new InvalidOperationException("Service thread has already been setup.");

            this.interval = interval;
            Keep = true;
            Watchdog = new ManualResetEvent(false);
            Servicethread = new Thread(new ThreadStart(Servicerun));

            Servicethread.Start();
        }

        void Servicerun()
        {
            while (Keep)
            {
                Watchdog.WaitOne(interval);
                SinceLastTimeTick();
                this.Flush();
            }
        }

        public virtual void SinceLastTimeTick()
        {
            if (DateTime.Now.Subtract(Lasttick).Minutes > 59)
            {
                System.Diagnostics.Trace.Write("Application is running.", "Tick");
                Lasttick = DateTime.Now;
            }
        }

        public override void Flush()
        {
            base.Flush();

            LogEvent outev;

            if (LogEventQueue.Count < 1)
                return;

            // todo try catch here
            // todo critical lock here

            if (schemadt == null)
                schemadt = new DataTable();

            lock (schemadt)
                using (var con = GetConnection())
                using (var cmd = new SqlCommand(SQLDML_Trace, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandTimeout = SQLCNF_UpdateTimeout;

                    if (schemadt.Columns.Count < 1)
                        da.Fill(schemadt);

                    // build the upldate command if we dont have it already
                    if (updatecmd == null)
                        using (var q = new SqlCommandBuilder(da))
                            updatecmd = q.GetInsertCommand();

                    updatecmd.Connection = con;
                    updatecmd.CommandTimeout = SQLCNF_UpdateTimeout;

                    using (var dt = schemadt.Clone())
                    {
                        da.UpdateBatchSize = 50;

                        // creating a datatable based on concurrent queue item
                        while (LogEventQueue.TryDequeue(out outev))
                            dt.Rows.Add(null, outev.Date, outev.Timestamp, outev.Session, outev.UserID, null, outev.Source, 1, outev.Data, null);

                        // if too many records do in batch
                        if (dt.Rows.Count < 600)
                        {
                            da.InsertCommand = updatecmd;
                            da.Update(dt);
                        }
                        else
                        {
                            using (var blk = new SqlBulkCopy(con))
                            {
                                blk.BulkCopyTimeout = SQLCNF_UpdateTimeout;

                                con.Open();
                                blk.BatchSize = 1500;
                                blk.DestinationTableName = "KST_EZC_Trace";
                                blk.WriteToServer(dt);
                            }
                        }

                    }
                }
        }

        public void Finish()
        {
            Keep = false;

            if (Watchdog != null)
                Watchdog.Set();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                int wait = 60;

                // give 60 seconds to dump everything
                if (Servicethread != null)
                    while (Servicethread.IsAlive)
                        if (--wait > 0)
                            Thread.Sleep(1000);
                        else
                            Servicethread.Abort();

                if (Watchdog != null)
                    Watchdog.Dispose();

                if (schemadt != null)
                    schemadt.Dispose();

                if (updatecmd != null)
                    updatecmd.Dispose();
            }

            base.Dispose(disposing);
        }


    }
}
