using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class ConfigurationManager
    {
        private static System.Reflection.Assembly _ExecutingAssembly;
        public static System.Reflection.Assembly ExecutingAssembly
        {
            get
            {
                return _ExecutingAssembly != null ? _ExecutingAssembly : _ExecutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            }
        }

        internal static void ApplySettings(object settings)
        {
            var tProps = System.Configuration.ConfigurationManager.AppSettings.AllKeys.ToList()
                .Select(row => new { Name = row, Value = System.Configuration.ConfigurationManager.AppSettings[row] })
                .ToList();

            var props = new List<System.Reflection.PropertyInfo>(settings.GetType().GetProperties());

            tProps.ForEach(dbprop =>
            {
                System.Diagnostics.Trace.TraceInformation("Local configuration {0} {1}", dbprop.Name, dbprop.Value);

                var prop = props.FirstOrDefault(p => p.Name == dbprop.Name);

                string Value = dbprop.Value;

                var mt = System.Text.RegularExpressions.Regex.Match(dbprop.Value, @"decrypt\((.*)\)");
                if (mt.Groups.Count == 2)
                    Value = new Library.Cryptography.Cryptography.AES(nameof(dbprop.Name)).Decrypt(mt.Groups[1].Value);

                if (prop == null)
                    throw new InvalidOperationException($"Property ({dbprop.Name}) has not been defined for object.");

                prop.SetValue(settings, Convert.ChangeType(Value, prop.PropertyType));
            });
        }

    }
}
