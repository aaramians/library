using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Converters.Excel
{
    public  class SerializableTypes
    {
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class cols
        {

            private colsCol[] colField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("col")]
            public colsCol[] col
            {
                get
                {
                    return this.colField;
                }
                set
                {
                    this.colField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class colsCol
        {

            private byte minField;

            private byte maxField;

            private double widthField;

            private byte bestFitField;

            private bool bestFitFieldSpecified;

            private bool customWidthField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte min
            {
                get
                {
                    return this.minField;
                }
                set
                {
                    this.minField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte max
            {
                get
                {
                    return this.maxField;
                }
                set
                {
                    this.maxField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte bestFit
            {
                get
                {
                    return this.bestFitField;
                }
                set
                {
                    this.bestFitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool bestFitSpecified
            {
                get
                {
                    return this.bestFitFieldSpecified;
                }
                set
                {
                    this.bestFitFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool customWidth
            {
                get
                {
                    return this.customWidthField;
                }
                set
                {
                    this.customWidthField = value;
                }
            }
        }

    }


}
