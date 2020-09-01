using System.Collections.Generic;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.Devices
{
    /// <summary>
    /// Array serialization class
    /// </summary>
     public class NumeneraDevices
    {
        [XmlElement("Cypher")]
        public List<Cypher> Cyphers { get; set; }
        [XmlElement("Artefact")]
        public List<Artefact> Artefacts { get; set; }
    }
}
