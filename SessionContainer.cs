using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class SessionContainer
    {
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Session
        {

            private SessionEnvironment environmentField;

            private SessionAssembley assembleyField;

            private int versionField;

            /// <remarks/>
            public SessionEnvironment Environment
            {
                get
                {
                    return this.environmentField;
                }
                set
                {
                    this.environmentField = value;
                }
            }

            /// <remarks/>
            public SessionAssembley Assembley
            {
                get
                {
                    return this.assembleyField;
                }
                set
                {
                    this.assembleyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class SessionEnvironment
        {

            private string commandLineField;

            private string currentDirectoryField;

            private string machineNameField;

            private string userDomainNameField;

            private string userNameField;

            private bool userInteractiveField;

            private long tickCountField;

            private string oSVersionField;

            private System.DateTime nowField;

            private System.DateTime utcNowField;

            private string currentTimeZoneField;

            /// <remarks/>
            public string CommandLine
            {
                get
                {
                    return this.commandLineField;
                }
                set
                {
                    this.commandLineField = value;
                }
            }

            /// <remarks/>
            public string CurrentDirectory
            {
                get
                {
                    return this.currentDirectoryField;
                }
                set
                {
                    this.currentDirectoryField = value;
                }
            }

            /// <remarks/>
            public string MachineName
            {
                get
                {
                    return this.machineNameField;
                }
                set
                {
                    this.machineNameField = value;
                }
            }

            /// <remarks/>
            public string UserDomainName
            {
                get
                {
                    return this.userDomainNameField;
                }
                set
                {
                    this.userDomainNameField = value;
                }
            }

            /// <remarks/>
            public string UserName
            {
                get
                {
                    return this.userNameField;
                }
                set
                {
                    this.userNameField = value;
                }
            }

            /// <remarks/>
            public bool UserInteractive
            {
                get
                {
                    return this.userInteractiveField;
                }
                set
                {
                    this.userInteractiveField = value;
                }
            }

            /// <remarks/>
            public long TickCount
            {
                get
                {
                    return this.tickCountField;
                }
                set
                {
                    this.tickCountField = value;
                }
            }

            /// <remarks/>
            public string OSVersion
            {
                get
                {
                    return this.oSVersionField;
                }
                set
                {
                    this.oSVersionField = value;
                }
            }

            /// <remarks/>
            public System.DateTime Now
            {
                get
                {
                    return this.nowField;
                }
                set
                {
                    this.nowField = value;
                }
            }

            /// <remarks/>
            public System.DateTime UtcNow
            {
                get
                {
                    return this.utcNowField;
                }
                set
                {
                    this.utcNowField = value;
                }
            }

            /// <remarks/>
            public string CurrentTimeZone
            {
                get
                {
                    return this.currentTimeZoneField;
                }
                set
                {
                    this.currentTimeZoneField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class SessionAssembley
        {

            private string versionField;

            private string fullNameField;

            /// <remarks/>
            public string Version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }

            /// <remarks/>
            public string FullName
            {
                get
                {
                    return this.fullNameField;
                }
                set
                {
                    this.fullNameField = value;
                }
            }
        }


    }
}
