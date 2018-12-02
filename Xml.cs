using System.Collections.Generic;
using System.Xml.Serialization;

namespace DediServerCron
{
    public class Xml
    {
        [XmlRoot(ElementName = "property")]
        public class Property
        {
            [XmlAttribute(AttributeName = "name")] public string Name { get; set; }

            [XmlAttribute(AttributeName = "value")]
            public string Value { get; set; }
        }

        [XmlRoot(ElementName = "ServerSettings")]
        public class ServerSettings
        {
            [XmlElement(ElementName = "property")] public List<Property> Property { get; set; }
        }
    }
}