using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library
{
    public class ConnectionStringBuilder
    {
        private static System.Reflection.Assembly _ExecutingAssembly;
        public static System.Reflection.Assembly ExecutingAssembly
        {
            get
            {
                return _ExecutingAssembly != null ? _ExecutingAssembly : _ExecutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            }
        }

        private static System.Reflection.Assembly _EntryAssembly;
        public static System.Reflection.Assembly EntryAssembly
        {
            get
            {
                return _EntryAssembly != null ? _EntryAssembly : _EntryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            }
        }

        public static SqlConnectionStringBuilder SqlConnectionString (string ConnectionString)
        {
            SqlConnectionStringBuilder t = null;
            if (t == null)
            {
                t = new SqlConnectionStringBuilder(ConnectionString);

                t.ApplicationName = !string.IsNullOrEmpty(t.ApplicationName) ? t.ApplicationName : EntryAssembly.FullName;

                {
                    var matchtest = Regex.Match(t.DataSource, @"decrypt\((.*)\)");
                    if (matchtest.Groups.Count == 2)
                        t.DataSource = new Cryptography.Cryptography.AES(nameof(t.DataSource)).Decrypt(matchtest.Groups[1].Value);
                }

                {
                    var matchtest = Regex.Match(t.InitialCatalog, @"decrypt\((.*)\)");
                    if (matchtest.Groups.Count == 2)
                        t.InitialCatalog = new Cryptography.Cryptography.AES(nameof(t.InitialCatalog)).Decrypt(matchtest.Groups[1].Value);
                }

                {
                    var matchtest = Regex.Match(t.UserID, @"decrypt\((.*)\)");
                    if (matchtest.Groups.Count == 2)
                        t.UserID = new Cryptography.Cryptography.AES(nameof(t.UserID)).Decrypt(matchtest.Groups[1].Value);
                }

                {
                    var matchtest = Regex.Match(t.Password, @"decrypt\((.*)\)");
                    if (matchtest.Groups.Count == 2)
                        t.Password = new Cryptography.Cryptography.AES(nameof(t.Password)).Decrypt(matchtest.Groups[1].Value);
                }

                t.PersistSecurityInfo = false;
            }
            return t;
        }

    }
}
