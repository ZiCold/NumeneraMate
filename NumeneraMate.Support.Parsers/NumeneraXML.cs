using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace NumeneraMate.Support.Parsers
{
    public static class NumeneraXML
    {
        /// <summary>
        /// Helper: Get cyphers list from generated XML
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<Cypher> DeserializeCyphersListFromXML(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(NumeneraDevices));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var xmlWrapper = (NumeneraDevices)ser.Deserialize(fs);
                return xmlWrapper.Cyphers;
            }
        }

        public static List<Artefact> DeserializeArtefactsListFromXML(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(NumeneraDevices));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var xmlWrapper = (NumeneraDevices)ser.Deserialize(fs);
                return xmlWrapper.Artefacts;
            }
        }

        public static void SerializeToXml(List<Cypher> cyphersList, string fileName)
        {
            var xmlDevices = new NumeneraDevices() { Cyphers = cyphersList };
            SerializeToXML(xmlDevices, fileName);
        }

        public static void SerializeToXML(NumeneraDevices xmlDevices, string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(NumeneraDevices));
            using (FileStream writer = new FileStream(fileName, FileMode.Create))
            //using (TextWriter writer = new StreamWriter(fileName))
            {
                ser.Serialize(writer, xmlDevices);
            }
        }
    }
}
