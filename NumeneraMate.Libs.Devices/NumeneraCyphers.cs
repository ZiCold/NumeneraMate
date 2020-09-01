using System.Collections.Generic;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.Devices
{
    public class NumeneraCyphers
    {
        [XmlElement("Cypher")]
        public List<Cypher> Cyphers { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("quantity")]
        public int Quantity { get => Cyphers.Count; }
    }

    public class NumeneraDevices
    {
        [XmlElement("Cypher")]
        public List<Cypher> Cyphers { get; set; }
        [XmlElement("Artefact")]
        public List<Artefact> Artefacts { get; set; }
    }
}
