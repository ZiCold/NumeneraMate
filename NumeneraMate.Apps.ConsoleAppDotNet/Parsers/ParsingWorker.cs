using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleAppDotNet.Parsers
{
    public static class ParsingWorker
    {
        public static void ParseCyphersToXML()
        {
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\APPs_InWorkFiles\";
            var name = "RAW_Extreme_Cyphers.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = fileName + "_xml.xml";
            var deviceParser = new DevicesParser("Extreme Cyphers", DeviceTypeEnum.Cypher);
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
            var deviceParser = new DevicesParser("Compendium", DeviceTypeEnum.Artefact);
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
            var deviceParser = new DevicesParser("Compendium", DeviceTypeEnum.Oddity);
            deviceParser.CreateXMLFromRawOddities(fileName, fileNameXml);

        }
    }
}
