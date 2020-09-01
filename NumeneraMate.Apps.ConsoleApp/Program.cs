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
			//HTMLTableFromXLSXCreator.Transform();
			GenerateDevices();

			Console.WriteLine();
            Console.WriteLine("Press anykey man");
            Console.ReadLine();
		}

		private static void GenerateDevices()
        {
			var cyphersList = new List<Cypher>();

			ConsoleKeyInfo c = new ConsoleKeyInfo();
            while (c.Key != ConsoleKey.Escape)
            {
                Console.WriteLine($"Generate from {cyphersList.Count}!");
				Console.WriteLine("Press G to generate or ESC to exit");
				c = Console.ReadKey();
                if (c.KeyChar == 'g')
                {
                    var rand = new Random(Guid.NewGuid().GetHashCode());
                    var rndInt = rand.Next(cyphersList.Count);
                    Console.WriteLine($"1d6 = {rand.Next(1, 6)}");
                    Console.WriteLine(cyphersList[rndInt].ToString());
                }
            }
        }

		#region TestMethods

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

		public static void CombineAllCyphers()
		{
			// combine them all
			var directory = @"E:\Documents\Tabletop RPGs\Numenera\APPs\NumeneraDevicesXML\Cyphers_";
			var files = new List<string>() { "Discovery.xml", "Destiny.xml", "Compendium.xml" };

			var allCyphers = new NumeneraDevices() { Cyphers = new List<Cypher>() };
			foreach (var file in files)
			{
				var filename = directory + file;
				var cyphers = NumeneraXML.DeserializeCyphersListFromXML(filename);
				allCyphers.Cyphers.AddRange(cyphers);
			}

			//DevicesParser.SerializeCyphersToXml(allCyphers.Cyphers, directory + $"_All_{allCyphers.Cyphers.Count}.xml");
		}

		public static void CombineAllArtefacts()
		{
			// combine them all
			var directory = @"E:\Documents\Tabletop RPGs\Numenera\APPs\NumeneraDevicesXML\Artefacts_";
			var files = new List<string>() { "Discovery.xml", "Destiny.xml", "Compendium.xml" };

			var allCyphers = new NumeneraDevices() { Artefacts = new List<Artefact>() };
			foreach (var file in files)
			{
				var filename = directory + file;
				var artefacts = NumeneraXML.DeserializeArtefactsListFromXML(filename);
				allCyphers.Artefacts.AddRange(artefacts);
			}

			NumeneraXML.SerializeToXml(allCyphers.Artefacts, directory + $"All_{allCyphers.Artefacts.Count}.xml");
		}

		#endregion TestMethods
	}
}
