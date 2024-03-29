﻿using NumeneraMate.Libs.NumeneraObjects.Devices;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NumeneraMate.Support.Parsers.FromRawText
{
    public static class NumeneraDevicesXML
    {
        #region Deserialize
        /// <summary>
        /// Helper: Get cyphers list from generated XML
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<Cypher> DeserializeCyphersListFromXML(string fileName)
        {
            return DeserializeDevicesFromXML(fileName).Cyphers;
        }

        public static List<Artefact> DeserializeArtefactsListFromXML(string fileName)
        {
            return DeserializeDevicesFromXML(fileName).Artefacts;
        }

        public static List<Oddity> DeserializeOdditiesListFromXML(string fileName)
        {
            return DeserializeDevicesFromXML(fileName).Oddities;
        }

        private static NumeneraDevices DeserializeDevicesFromXML(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(NumeneraDevices));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var xmlWrapper = (NumeneraDevices)ser.Deserialize(fs);
                return xmlWrapper;
            }
        }

        public static Cypher DeserializeCypherFromXMLString(string cypherXML)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Cypher));
            using (TextReader reader = new StringReader(cypherXML))
            {
                return (Cypher)ser.Deserialize(reader);
            }
        }

        #endregion

        #region Serialize
        public static void SerializeToXml(List<Cypher> cyphersList, string fileName)
        {
            var xmlDevices = new NumeneraDevices(cyphersList);
            SerializeToXml(xmlDevices, fileName);
        }

        public static void SerializeToXml(List<Artefact> artefactsList, string fileName)
        {
            var xmlDevices = new NumeneraDevices(artefactsList);
            SerializeToXml(xmlDevices, fileName);
        }

        public static void SerializeToXml(List<Oddity> odditiesList, string fileName)
        {
            var xmlDevices = new NumeneraDevices() { Oddities = odditiesList, Count = odditiesList.Count };
            SerializeToXml(xmlDevices, fileName);
        }

        private static void SerializeToXml(NumeneraDevices xmlDevices, string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(NumeneraDevices));
            using (FileStream writer = new FileStream(fileName, FileMode.Create))
            //using (TextWriter writer = new StreamWriter(fileName))
            {
                ser.Serialize(writer, xmlDevices);
            }
        }
        #endregion


        /// <summary>
        /// View all unique attributes to create specific class
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="elementName"></param>
        private static void ViewUniqueAttributes(string xmlFile, string elementName)
        {
            XDocument doc = XDocument.Load(xmlFile);
            var uniqueElements = new List<string>();
            foreach (var elem in doc.Elements().First().Elements(elementName))
            {
                // elements() returns direct children
                // descendants() recurses
                foreach (var attr in elem.Elements())
                {
                    var name = attr.Name.ToString();
                    if (!uniqueElements.Contains(name))
                        uniqueElements.Add(name);
                }
            }
            uniqueElements.ForEach(x => System.Console.WriteLine(x + " "));
        }
    }
}
