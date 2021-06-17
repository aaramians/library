using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Library.Serializers
{
    public static class SerializerExtensions
    {
        /// <summary>
        /// Converts an anonymous type to an XElement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns the object as it's XML representation in an XElement.</returns>
        public static XElement ToXml2(this object input)
        {
            return input.ToXml2(null);
        }

        /// <summary>
        /// Converts an anonymous type to an XElement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="element">The element name.</param>
        /// <returns>Returns the object as it's XML representation in an XElement.</returns>
        public static XElement ToXml2(this object input, string element)
        {
            return _ToXml(input, element);
        }

        private static XElement _ToXml(object input, string element, int? arrayIndex = null, string arrayName = null)
        {
            if (input == null)
                return null;

            if (String.IsNullOrEmpty(element))
            {
                string name = input.GetType().Name;
                element = name.Contains("AnonymousType")
                    ? "Object"
                    : arrayIndex != null
                        ? arrayName + "_" + arrayIndex
                        : name;
            }

            element = XmlConvert.EncodeName(element);
            var ret = new XElement(element);

            if (input != null)
            {
                var type = input.GetType();
                var props = type.GetProperties();

                var elements = props.Select(p =>
                {
                    var pType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    var name = XmlConvert.EncodeName(p.Name);
                    var val = pType.IsArray ? "array" : p.GetValue(input, null);
                    var value = pType.IsEnumerable()
                        ? GetEnumerableElements(p, (IEnumerable)p.GetValue(input, null))
                        : pType.IsSimpleType2() || pType.IsEnum
                            ? new XElement(name, val)
                            : val.ToXml2(name);
                    return value;
                })
                .Where(v => v != null);

                ret.Add(elements);
            }

            return ret;
        }

        #region helpers

        private static XElement GetEnumerableElements(PropertyInfo info, IEnumerable input)
        {
            var name = XmlConvert.EncodeName(info.Name);

            XElement rootElement = new XElement(name);

            int i = 0;
            foreach (var v in input)
            {
                XElement childElement = v.GetType().IsSimpleType2() || v.GetType().IsEnum ? new XElement(name + "_" + i, v) : _ToXml(v, null, i, name);
                rootElement.Add(childElement);
                i++;
            }
            return rootElement;
        }

        private static readonly Type[] WriteTypes = new[] {
    typeof(string), typeof(DateTime), typeof(Enum),
    typeof(decimal), typeof(Guid),
};
        public static bool IsSimpleType2(this Type type)
        {
            return type.IsPrimitive || WriteTypes.Contains(type);
        }

        private static readonly Type[] FlatternTypes = new[] {
    typeof(string)
};
        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && !FlatternTypes.Contains(type);
        }
        #endregion
    }


}
