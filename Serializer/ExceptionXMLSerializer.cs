using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Library.Serializers
{
    public static class ExceptionXMLSerializer
    {
        public static string XMLExcecption(Exception ex)
        {
            using (var sw = new MemoryStream())
            using (var xmw = new XmlTextWriter(sw, new ASCIIEncoding()) { Formatting = Formatting.Indented, Indentation = 4 })
            {
                xmw.Formatting = Formatting.Indented;
                xmw.Indentation = 4;
                XMLExcecption(ex, xmw);
                xmw.Flush();
                xmw.Close();
                return Encoding.ASCII.GetString(sw.ToArray());
            }
        }

        public static void XMLExcecption(Exception ex, XmlWriter xw, int dp = 0)
        {
            if (dp == 0)
            {
                xw.WriteStartDocument();
                XMLExcecption(ex, xw, dp + 1);
                xw.WriteEndDocument();
                return;
            }
            else if (dp == 1)
            {
                xw.WriteStartElement("Exceptions");
                XMLExcecption(ex, xw, dp + 1);
                xw.WriteEndElement();
                return;
            }

            xw.WriteStartElement("Exception");
            xw.WriteAttributeString("Type", ex.GetType().ToString());

            if (ex.Source != null)
                xw.WriteAttributeString("Source", ex.Source);

            if (ex.Message != null)
                xw.WriteElementString("Message", ex.Message);

            if (ex.StackTrace != null)
                xw.WriteElementString("StackTrace", ex.StackTrace);

            // TODO complete target-site dumping
            // Target Site  --------------------------------------------
            if (ex.TargetSite != null)
            {
                xw.WriteStartElement("TargetSite");
                xw.WriteAttributeString("Site", ex.TargetSite.ToString());
                xw.WriteElementString("DeclaringType", ex.TargetSite.DeclaringType.ToString());
                xw.WriteElementString("Assembly", ex.TargetSite.DeclaringType.Assembly.ToString());
                xw.WriteEndElement();
            }

            // date extraction -----------------------------------------
            if (ex.Data != null && ex.Data.Count > 0)
            {
                xw.WriteStartElement("Data");
                foreach (var key in ex.Data.Keys)
                {
                    xw.WriteStartElement("Datum", key.ToString());
                    xw.WriteAttributeString("KType", key.GetType().ToString());
                    xw.WriteAttributeString("VType", ex.Data[key].GetType().ToString());
                    xw.WriteElementString("Value", ex.Data[key].ToString());
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
            }

            if (ex.InnerException != null)
                XMLExcecption(ex.InnerException, xw, dp + 1);

            xw.WriteEndElement();
        }


    }

}
