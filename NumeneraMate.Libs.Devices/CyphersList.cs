using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.Devices
{
    [Serializable, XmlRoot("Cyphers")]
    public class CyphersList
    {
        [XmlElement("Cypher")]
        public List<Cypher> Cyphers { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("quantity")]
        public string Quantity { get; set; }
    }
}
