using NumeneraMate.Support.Parsers;
using NumeneraMate.Libs.NumeneraObjects.Devices;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace NumeneraMate.Apps.ConsoleAppDotNet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CombineAllArtefacts();
            Console.WriteLine("Press anykey man");
        }

        #region Devices Parsing

        /// <summary>
        /// 1. Get devices list from raw text (from pdf)
        /// </summary>
        private static void ParseRawDevicesToJson()
        {
            var directory = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\raw\";
            var filename = "building_tomorrow_artefacts_raw.txt";
            var resultName = directory + Path.GetFileNameWithoutExtension(filename) + ".json";
            var source = "Building Tomorrow";
            var type = DeviceTypeEnum.Artefact;


            string json = "";
            var deviceParser = new DevicesParser();
            switch (type)
            {
                case DeviceTypeEnum.Cypher:
                    var cyphersList = deviceParser.GetCyphersListFromRawText(directory + filename, source);
                    json = JsonSerializer.Serialize(cyphersList, GetJsonOptions());
                    break;
                case DeviceTypeEnum.Artefact:
                    var artefactsList = deviceParser.GetArtefactsListFromRawText(directory + filename, source);
                    json = JsonSerializer.Serialize(artefactsList, GetJsonOptions());
                    break;
                case DeviceTypeEnum.Oddity:
                    var oddities = deviceParser.GetOdditiesListFromRawText(directory + filename, source);
                    json = JsonSerializer.Serialize(oddities, GetJsonOptions());
                    break;
            }
            File.WriteAllText(resultName, json);
        }

        /// <summary>
        /// 2. Combine cyphers with other cyphers
        /// </summary>
        public static void CombineAllCyphers()
        {
            var fileNames = new List<string>()
            {
                @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\Cyphers_Official_Books.json",
                @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\raw\priests_cyphers_raw.json",
                @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\raw\building_tomorrow_cyphers_raw.json"
            };
            var resultName = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\Cyphers_Official_Books_Update.json";
            var cyphers = new List<Cypher>();
            foreach (var file in fileNames)
            {
                var currentCyphers = JsonSerializer.Deserialize<List<Cypher>>(File.ReadAllText(file));
                Console.WriteLine($"{currentCyphers.Count} from {file}");
                cyphers.AddRange(currentCyphers);
            }

            var json = JsonSerializer.Serialize(cyphers, GetJsonOptions());
            File.WriteAllText(resultName, json);
        }

        /// <summary>
        /// 2. Combine artefacts with other artefacts
        /// </summary>
        public static void CombineAllArtefacts()
        {
            var fileNames = new List<string>()
            {
                @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\Artefacts_Official_Books.json",
                @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\raw\priests_artefacts_raw.json",
                @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\raw\building_tomorrow_artefacts_raw.json"
            };
            var resultName = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\Artefacts_Official_Books_Update.json";
            var artefacts = new List<Artefact>();
            foreach (var file in fileNames)
            {
                var currentArtefacts = JsonSerializer.Deserialize<List<Artefact>>(File.ReadAllText(file));
                Console.WriteLine($"{currentArtefacts.Count} from {file}");
                artefacts.AddRange(currentArtefacts);
            }

            var json = JsonSerializer.Serialize(artefacts, GetJsonOptions());
            File.WriteAllText(resultName, json);
        }


        public static void AddCategories()
        {
            var cyphersFileName = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\Cyphers_Official_Books.json";
            var cyphersLlist = JsonSerializer.Deserialize<List<Cypher>>(File.ReadAllText(cyphersFileName));
            Console.WriteLine(cyphersLlist.Count + " cyphers loaded");

            var artefactsFileName = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\Artefacts_Official_Books.json";
            var artefactsList = JsonSerializer.Deserialize<List<Artefact>>(File.ReadAllText(artefactsFileName));
            Console.WriteLine(artefactsList.Count + " artefacts loaded");

            var fileWithCategoriesList = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\devices\xmls\compendium_categories.txt";
            var currentCategory = "";
            foreach (var line in File.ReadAllLines(fileWithCategoriesList))
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("Category:"))
                {
                    currentCategory = line.Substring("Category:".Length).Trim();
                }
                else
                {
                    var indexDevice = line.IndexOf("Cypher SA");
                    if (indexDevice == -1) indexDevice = line.IndexOf("Cypher CB");
                    if (indexDevice != -1)
                    {
                        var cypherName = line.Substring(0, indexDevice - 1).Trim().ToLower();
                        var cypher = cyphersLlist.FirstOrDefault(x => x.Name.ToLower() == cypherName);
                        if (cypher != null)
                        {
                            if (string.IsNullOrEmpty(cypher.Categories))
                                cypher.Categories = currentCategory;
                            else
                                cypher.Categories += ", " + currentCategory;
                        }
                    }
                    else
                    {
                        indexDevice = line.IndexOf("Artifact SA");
                        if (indexDevice == -1) indexDevice = line.IndexOf("Artifact CB");
                        if (indexDevice != -1)
                        {
                            var artefactName = line.Substring(0, line.Length - indexDevice).Trim().ToLower();
                            var artefact = artefactsList.FirstOrDefault(x => x.Name.ToLower() == artefactName);
                            if (artefact != null)
                            {
                                if (string.IsNullOrEmpty(artefact.Categories))
                                    artefact.Categories = currentCategory;
                                else
                                    artefact.Categories += ", " + currentCategory;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unprocessed line: " + line);
                        }
                    }
                }
            }
            Console.WriteLine("Processing done");

            // serialization
            var jsonCyphersWithCategories = JsonSerializer.Serialize(cyphersLlist, new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(cyphersFileName.Replace(".json", "_WithCategories.json"), jsonCyphersWithCategories);

            var jsonArtefactsWithCategories = JsonSerializer.Serialize(artefactsList, new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(artefactsFileName.Replace(".json", "_WithCategories.json"), jsonArtefactsWithCategories);
        }


        #endregion

        public static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                //Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        }

    }
}