using NPOI.SS.Formula.Functions;
using NumeneraMate.Libs.Devices;
using NumeneraMate.Support.Parsers;
using NumeneraMate.Libs.Creatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NumeneraMate.Apps.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // todo: разобраться с парсерами
            //NumeneraParsersProject.MainMethod();
            // ParseCyphersToXML();

            // var eventsDirectory = @"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\HexCampaign\";
            //var eventsXlsxName = @"Creatures and Events Table.xlsx";
            //XMLfromXLSXCreator.CreateXMLsFromEventsTable(eventsDirectory, eventsXlsxName);
            //GenerateCreatures(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\NumeneraAppFiles\", @"Creatures and Events Table.xlsx");
            //GenerateDevices(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\NumeneraAppFiles\Devices");

            //var smth = CreaturesParser.GetCreaturesListFromExcel();
            //CombineAllCyphers();
            //CombineAllArtefacts();

            //HTMLTableFromXLSXCreator.Transform();

            //TestCalculatedProperties();

            // CreateJsonFromFocusesExcel.Generate();

            Console.WriteLine("Press anykey man");
            Console.ReadLine();
        }

        #region TestMethods

        public static void ParseCyphersToXML()
        {
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\APPs_InWorkFiles\";
            var name = "RAW_Extreme_Cyphers.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = fileName + "_xml.xml";
            var deviceParser = new DevicesParser("Extreme Cyphers", DeviceType.Cypher);
            deviceParser.CreateXMLFromRawCyphersText(fileName, fileNameXml);
            var cyphers = NumeneraXML.DeserializeCyphersListFromXML(fileNameXml);
            cyphers.ForEach(x => Console.WriteLine(x));
        }

        public static void ParseArtefactsToXML()
        {
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\APPs\Artefacts\";
            var name = "RAW_Artefacts_Compendium.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = fileName + "_xml.xml";
            var deviceParser = new DevicesParser("Compendium", DeviceType.Artefact);
            deviceParser.CreateXMLFromRawArtefactsText(fileName, fileNameXml);
            var cyphers = NumeneraXML.DeserializeArtefactsListFromXML(fileNameXml);
            cyphers.ForEach(x => Console.WriteLine(x));
        }

        public static void ParseOddities()
        {
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\APPs\Oddities\";
            var name = "RAW_Oddities_Compendium.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = fileName + "_xml.xml";
            var deviceParser = new DevicesParser("Compendium", DeviceType.Oddity);
            deviceParser.CreateXMLFromRawOddities(fileName, fileNameXml);

        }

        public static void CombineAllCyphers()
        {
            // combine them all
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\NumeneraAppFiles\Devices\Cyphers_";
            var files = new List<string>() { "Discovery.xml", "Destiny.xml", "Compendium.xml" };

            var allCyphers = new List<Cypher>();
            foreach (var file in files)
            {
                var filename = directory + file;
                var cyphers = NumeneraXML.DeserializeCyphersListFromXML(filename);
                NumeneraXML.SerializeToXml(cyphers, directory + $"{cyphers[0].Source}.xml");
                allCyphers.AddRange(cyphers);
            }

            var cyphersToSerialize = new NumeneraDevices(allCyphers);
            NumeneraXML.SerializeToXml(cyphersToSerialize.Cyphers, directory + $"AllSources_{cyphersToSerialize.Cyphers.Count}.xml");
        }

        public static void CombineAllArtefacts()
        {
            // combine them all
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\NumeneraAppFiles\Devices\Artefacts_";
            var files = new List<string>() { "Discovery.xml", "Destiny.xml", "Compendium.xml" };

            var allArtefacts = new List<Artefact>();
            foreach (var file in files)
            {
                var filename = directory + file;
                var artefacts = NumeneraXML.DeserializeArtefactsListFromXML(filename);
                NumeneraXML.SerializeToXml(artefacts, directory + $"{artefacts[0].Source}_fixed.xml");
                allArtefacts.AddRange(artefacts);
            }

            var artefactsToSerialize = new NumeneraDevices(allArtefacts);
            NumeneraXML.SerializeToXml(artefactsToSerialize.Artefacts, directory + $"AllSources_{artefactsToSerialize.Artefacts.Count}.xml");
        }

        #endregion TestMethods
    }
}
