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

namespace NumeneraMate.Apps.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateCreatures(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\NumeneraAppFiles\", @"Creatures and Events Table.xlsx");
            //GenerateDevices(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\NumeneraAppFiles\Devices");
            
            
            //var smth = CreaturesParser.GetCreaturesListFromExcel();
            //CombineAllCyphers();
            //CombineAllArtefacts();
            //HTMLTableFromXLSXCreator.Transform();
            //TestCalculatedProperties();

            //Console.WriteLine();
            //Console.WriteLine("Press anykey man");
            //Console.ReadLine();
        }

        private static void GenerateDevices(string directory)
        {
            var cyphersFileName = Path.Combine(directory, "Cyphers_AllSources.xml");
            var cyphersList = NumeneraXML.DeserializeCyphersListFromXML(cyphersFileName);
            Console.WriteLine("Cyphers loaded: " + cyphersList.Count);

            var artefactsFileName = Path.Combine(directory, "Artefacts_AllSources.xml");
            var artefactsList = NumeneraXML.DeserializeArtefactsListFromXML(artefactsFileName);
            Console.WriteLine("Artefacts loaded: " + artefactsList.Count);

            var odditiesFilename = Path.Combine(directory, "Oddities_AllSources.xml");
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

        public static void GenerateCreatures(string directory, string filename)
        {
            var creatures = CreaturesParser.GetCreaturesListFromExcel(directory, filename);
            var endlessCreatures = creatures.Where(x => x.UsedInEndlessLegendCampaign).ToList();
            Console.WriteLine($"Creatures loaded: {creatures.Count}");
            Console.WriteLine($"Of which EndlessLegends creatures: {endlessCreatures.Count}");

            var rand = new Random(Guid.NewGuid().GetHashCode());
            int randomIndex = 0;
            var currentCreatures = new List<Creature>();
            var output = "";

            var c = new ConsoleKeyInfo();

            while (c.Key != ConsoleKey.Escape)
            {
                var terrainList = new List<string>()
                {
                    "Ruins/Underground",
                    "Plains/Hills",
                    "Desert",
                    "Woods",
                    "Mountains",
                    "Swamp",
                    "Dimensions",
                    "Water"
                };
                Console.WriteLine("Choose terrain type: ");
                for (int i = 0; i < terrainList.Count; i++)
                {
                    Console.Write($"{i + 1} - {terrainList[i]}");
                    if (i != terrainList.Count - 1) Console.Write(", ");
                    else Console.WriteLine();
                }
                Console.WriteLine();

                c = Console.ReadKey(true);
                switch (c.KeyChar)
                {
                    case '1':
                        currentCreatures = endlessCreatures.Where(x => x.RuinsUnderground).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
                        break;
                    case '2':
                        currentCreatures = endlessCreatures.Where(x => x.PlainsHills).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
                        break;
                    case '3':
                        currentCreatures = endlessCreatures.Where(x => x.Desert).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
                        break;
                    case '4':
                        currentCreatures = endlessCreatures.Where(x => x.Woods).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
                        break;
                    case '5':
                        currentCreatures = endlessCreatures.Where(x => x.Mountains).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
                        break;
                    case '6':
                        currentCreatures = endlessCreatures.Where(x => x.Swamp).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
                        break;
                    case '7':
                        currentCreatures = endlessCreatures.Where(x => x.Dimensions).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
                        break;
                    case '8':
                        currentCreatures = endlessCreatures.Where(x => x.Water).ToList();
                        randomIndex = rand.Next(currentCreatures.Count);
                        output = $"Terrain type: {terrainList[int.Parse(c.KeyChar.ToString()) - 1]}" + Environment.NewLine +
                            $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                            currentCreatures[randomIndex].ToString() + Environment.NewLine;
                        Console.WriteLine(output);
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
