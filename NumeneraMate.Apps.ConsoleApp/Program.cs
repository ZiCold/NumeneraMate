using NPOI.SS.Formula.Functions;
using NumeneraMate.Libs.Devices;
using NumeneraMate.Support.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NumeneraMate.Apps.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CombineAllCyphers();
            //CombineAllArtefacts();
            //HTMLTableFromXLSXCreator.Transform();
            //GenerateDevices();
            //TestCalculatedProperties();

            //Console.WriteLine();
            //Console.WriteLine("Press anykey man");
            //Console.ReadLine();
        }

        private static void GenerateDevices()
        {
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\NumeneraAppFiles\Devices";

            var cyphersFileName = Path.Combine(directory, "Cyphers_All_538.xml");
            var cyphersList = NumeneraXML.DeserializeCyphersListFromXML(cyphersFileName);
            Console.WriteLine("Cyphers loaded: " + cyphersList.Count);

            var artefactsFileName = Path.Combine(directory, "Artefacts_All_336.xml");
            var artefactsList = NumeneraXML.DeserializeArtefactsListFromXML(artefactsFileName);
            Console.WriteLine("Artefacts loaded: " + artefactsList.Count);

            var odditiesFilename = Path.Combine(directory, "Oddities_All_400.xml");
            var odditiesList = NumeneraXML.DeserializeOdditiesListFromXML(odditiesFilename);
            Console.WriteLine("Oddities loaded: " + odditiesList.Count);

            var rand = new Random(Guid.NewGuid().GetHashCode());
            int randomIndex = 0, diceRandom = 1;

            ConsoleKeyInfo c = new ConsoleKeyInfo();
            // TODO: D6/D10 random
            // TODO: Calculate final level automatically
            // TODO: RollTable Random (find maximum number in the last element)
            // TODO: Owerwrite ToString() specific to device? Or create custom output for each device?
            while (c.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("******** Choose device to generate: 1 - Cypher; 2 - Artefact; 3 - Oddity;");
                c = Console.ReadKey(true);
                switch (c.KeyChar)
                {
                    case '1':
                        Console.WriteLine($"Generating from {cyphersList.Count} cyphers\n");
                        randomIndex = rand.Next(cyphersList.Count);
                        diceRandom = rand.Next(1, 6);
                        cyphersList[randomIndex].Level += $" [D6 = {diceRandom}]";
                        Console.WriteLine(cyphersList[randomIndex].ToString());
                        break;
                    case '2':
                        Console.WriteLine($"Generatring from {artefactsList.Count} artefacts\n");
                        randomIndex = rand.Next(artefactsList.Count);
                        diceRandom = rand.Next(1, 6);
                        artefactsList[randomIndex].Level += $" [D6 = {diceRandom}]";
                        Console.WriteLine(artefactsList[randomIndex].ToString());
                        break;
                    case '3':
                        Console.WriteLine($"Generating from {odditiesList.Count} oddities\n");
                        randomIndex = rand.Next(odditiesList.Count);
                        Console.WriteLine(odditiesList[randomIndex].ToString());
                        break;
                }
            }
        }

        #region TestMethods

        public static void ParseCyphersToXML()
        {
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\APPs\Cyphers\";
            var name = "RAW_Cyphers_Discovery.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = fileName + "_xml.xml";
            var deviceParser = new DevicesParser("Discovery", DeviceType.Cypher);
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

            var allCyphers = new NumeneraDevices() { Cyphers = new List<Cypher>() };
            foreach (var file in files)
            {
                var filename = directory + file;
                var cyphers = NumeneraXML.DeserializeCyphersListFromXML(filename);
                NumeneraXML.SerializeToXml(cyphers, directory + $"{cyphers[0].Source}_{cyphers.Count}_fixed.xml");
                allCyphers.Cyphers.AddRange(cyphers);
            }

            NumeneraXML.SerializeToXml(allCyphers.Cyphers, directory + $"All_{allCyphers.Cyphers.Count}.xml");
        }

        public static void CombineAllArtefacts()
        {
            // combine them all
            var directory = @"E:\Documents\Tabletop RPGs\Numenera\NumeneraAppFiles\Devices\Artefacts_";
            var files = new List<string>() { "Discovery.xml", "Destiny.xml", "Compendium.xml" };

            var allArtefacts = new NumeneraDevices() { Artefacts = new List<Artefact>() };
            foreach (var file in files)
            {
                var filename = directory + file;
                var artefacts = NumeneraXML.DeserializeArtefactsListFromXML(filename);
                //NumeneraXML.SerializeToXml(artefacts, directory + $"{artefacts[0].Source}_{artefacts.Count}_fixed.xml");
                allArtefacts.Artefacts.AddRange(artefacts);
            }

            NumeneraXML.SerializeToXml(allArtefacts.Artefacts, directory + $"All_{allArtefacts.Artefacts.Count}.xml");
        }

        public static void TestCalculatedProperties()
        {
            var d10cypherXML = @"  <Cypher>
    <Name>Analysis Scanner</Name>
    <Level>1d10</Level>
    <MinimumCraftingLevel>1</MinimumCraftingLevel>
    <Wearable>Bracelet</Wearable>
    <Usable>Handheld device</Usable>
    <Effect>This device scans and records everything within short range for one round and then conveys the level and nature of all creatures, objects, and energy sources it scanned. This information can be accessed for 28 hours after the scan.</Effect>
    <Source>Compendium</Source>
  </Cypher>";
            var d10cypher = NumeneraXML.DeserializeCypherFromXMLString(d10cypherXML);
            var d10baseLevel = d10cypher.LevelBase;
            var d10levelTerm = d10cypher.LevelTerm;
            var d10minCraftingLevel = d10cypher.MinimumCraftingLevel;

            var d6cypherXML = @"  <Cypher>
    <Name>Amplification Parasite</Name>
    <Level>1d6 + 4</Level>
    <MinimumCraftingLevel>4</MinimumCraftingLevel>
    <Internal>Living fish, beetle, or worm that must be ingested</Internal>
    <Effect>Upon eating the parasite, the user chooses one stat and the GM chooses a different stat. The difficulty of any roll related to the user’s chosen stat is reduced by two steps, and the difficulty of any roll involving the GM’s chosen stat is increased by two steps. The parasite dies after 1d6 hours, and the effect ends when the user violently expels it from her body.</Effect>
    <Source>Compendium</Source>
  </Cypher>";
            var d6cypher = NumeneraXML.DeserializeCypherFromXMLString(d6cypherXML);
            var d6baseLevel = d6cypher.LevelBase;
            var d6levelTerm = d6cypher.LevelTerm;
            var d6minCraftingLevel = d6cypher.MinimumCraftingLevel;

            Console.WriteLine();
        }

        #endregion TestMethods
    }
}
