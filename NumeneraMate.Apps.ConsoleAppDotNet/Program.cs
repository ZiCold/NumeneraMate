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