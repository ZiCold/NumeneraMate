using System.Collections.Generic;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.NumeneraObjects.Devices
{
    /// <summary>
    /// Array serialization class
    /// </summary>
     public class NumeneraDevices
    {
        public NumeneraDevices() { }
        public NumeneraDevices(List<Cypher> cypherList)
        {
            Cyphers = cypherList;
            Count = cypherList.Count;
        }
        public NumeneraDevices(List<Artefact> artefactsList)
        {
            Artefacts = artefactsList;
            Count = artefactsList.Count;
        }
        public NumeneraDevices(List<Oddity> odditiesList)
        {
            Oddities = odditiesList;
            Count = odditiesList.Count;
        }

        [XmlElement("Cypher")]
        public List<Cypher> Cyphers { get; set; }
        [XmlElement("Artefact")]
        public List<Artefact> Artefacts { get; set; }

        [XmlElement("Oddity")]
        public List<Oddity> Oddities { get; set; }
        [XmlAttribute("Count")]
        public int Count { get; set; }
    }
}
