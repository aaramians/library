using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Serializers
{
    public partial class XmlSerializers
    {
        public static void Serialize<ObjectType>(FileInfo file, object Source)
        {
            var xser = new System.Xml.Serialization.XmlSerializer(typeof(ObjectType));
            using (Stream s = new FileStream(file.FullName, FileMode.CreateNew))
                xser.Serialize(s, Source);
        }

        public static ObjectType Deserialize<ObjectType>(FileInfo file)
        {
            var xdser = new System.Xml.Serialization.XmlSerializer(typeof(ObjectType));
            using (Stream s = new FileStream(file.FullName, FileMode.Open))
                return (ObjectType)xdser.Deserialize(s);
        }

        public static string Serialize<ObjectType>(object Source)
        {
            var xser = new System.Xml.Serialization.XmlSerializer(typeof(ObjectType));
            using (var s = new MemoryStream())
            {
                xser.Serialize(s, Source);
                return Encoding.UTF8.GetString(s.ToArray());
            }
        }

        public static ObjectType Deserialize<ObjectType>(string source)
        {
            var xdser = new System.Xml.Serialization.XmlSerializer(typeof(ObjectType));
            using (Stream s = new MemoryStream(Encoding.UTF8.GetBytes(source)))
                return (ObjectType)xdser.Deserialize(s);
        }


        public class DynamicXml : DynamicObject
        {
            XElement _root;
            private DynamicXml(XElement root)
            {
                _root = root;
            }

            public static DynamicXml Parse(string xmlString)
            {
                return new DynamicXml(RemoveNamespaces(XDocument.Parse(xmlString).Root));
            }

            public static DynamicXml Load(string filename)
            {
                return new DynamicXml(RemoveNamespaces(XDocument.Load(filename).Root));
            }

            private static XElement RemoveNamespaces(XElement xElem)
            {
                var attrs = xElem.Attributes()
                            .Where(a => !a.IsNamespaceDeclaration)
                            .Select(a => new XAttribute(a.Name.LocalName, a.Value))
                            .ToList();

                if (!xElem.HasElements)
                {
                    XElement xElement = new XElement(xElem.Name.LocalName, attrs);
                    xElement.Value = xElem.Value;
                    return xElement;
                }

                var newXElem = new XElement(xElem.Name.LocalName, xElem.Elements().Select(e => RemoveNamespaces(e)));
                newXElem.Add(attrs);
                return newXElem;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = null;

                var att = _root.Attribute(binder.Name);
                if (att != null)
                {
                    result = att.Value;
                    return true;
                }

                var nodes = _root.Elements(binder.Name);
                if (nodes.Count() > 1)
                {
                    result = nodes.Select(n => n.HasElements ? (object)new DynamicXml(n) : n.Value).ToList();
                    return true;
                }

                var node = _root.Element(binder.Name);
                if (node != null)
                {
                    result = node.HasElements || node.HasAttributes ? (object)new DynamicXml(node) : node.Value;
                    return true;
                }

                return true;
            }
        }


    }

}
