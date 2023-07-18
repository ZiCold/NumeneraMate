using NumeneraMate.Libs.Creatures;
using NumeneraMate.Support.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp
{
    public static class GeneratorConsole
    {
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
                        var device = cyphersList[randomIndex];
                        //device.Level += $" [D6 = {diceRandom}]";
                        Console.WriteLine(device.ToString());
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
            var currentCreatures = new List<Creature>();

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
                        break;
                    case '2':
                        currentCreatures = endlessCreatures.Where(x => x.PlainsHills).ToList();
                        break;
                    case '3':
                        currentCreatures = endlessCreatures.Where(x => x.Desert).ToList();
                        break;
                    case '4':
                        currentCreatures = endlessCreatures.Where(x => x.Woods).ToList();
                        break;
                    case '5':
                        currentCreatures = endlessCreatures.Where(x => x.Mountains).ToList();
                        break;
                    case '6':
                        currentCreatures = endlessCreatures.Where(x => x.Swamp).ToList();
                        break;
                    case '7':
                        currentCreatures = endlessCreatures.Where(x => x.Dimensions).ToList();
                        break;
                    case '8':
                        currentCreatures = endlessCreatures.Where(x => x.Water).ToList();
                        break;
                }

                if (currentCreatures.Count != 0)
                    ShowCurrentRandomCreature(currentCreatures, terrainList[int.Parse(c.KeyChar.ToString()) - 1]);
                currentCreatures.Clear();
            }
        }

        public static void ShowCurrentRandomCreature(List<Creature> currentCreatures, string terrain)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var randomIndex = rand.Next(currentCreatures.Count);
            string output = $"Terrain type: {terrain}" + Environment.NewLine +
                $"Number of creatures: {currentCreatures.Count}" + Environment.NewLine +
                currentCreatures[randomIndex].ToString() + Environment.NewLine;
            Console.WriteLine(output);
        }
    }
}
