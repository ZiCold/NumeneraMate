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
			ParseCyphersToXML();
			//CombineAllCyphers();

			//HTMLTableFromXLSXCreator.Transform();
			//GenerateDevices();


			Console.WriteLine();
            Console.WriteLine("Press anykey man");
            Console.ReadLine();
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

			var allCyphers = new NumeneraCyphers() { Cyphers = new List<Cypher>() };
			foreach (var file in files)
			{
				var filename = directory + file;
				var cyphers = NumeneraXML.DeserializeCyphersListFromXML(filename);
				allCyphers.Cyphers.AddRange(cyphers);
			}

			//DevicesParser.SerializeCyphersToXml(allCyphers.Cyphers, directory + $"_All_{allCyphers.Cyphers.Count}.xml");
		}

		private static void GenerateDevices()
        {
			var dir = @"E:\Documents\Tabletop RPGs\Numenera\APPs\XMLFilesFinal\";

			var cyphersFilenames = new List<string>() { "Cyphers_Discovery", "Cyphers_Destiny", "Cyphers_Compendium" };
			var artefactsFilenames = new List<string> { "Artefacts_Discovery", "Artefacts_Destiny", "Artefacts_Compendium" };
			var odditiesFilenames = new List<string> { "Oddities_Discovery", "Oddities_Compendium" };

			var cyphersList = new List<Cypher>();
			foreach (var cyphersFile in cyphersFilenames)
			{
				var repo = new XMLRepo(dir + cyphersFile + ".xml");
				var currentCyphersList = repo.DeserializeXmlFile().Cyphers;
				cyphersList.AddRange(currentCyphersList);
				Console.WriteLine($"{cyphersFile} cyphers loaded: {currentCyphersList.Count}");
				//cyphersList.ForEach(x => System.Console.WriteLine(x.ToString()));
			}
            Console.WriteLine($"Total cyphers count: {cyphersList.Count}");

			//ViewUniqueAttributes(dir + fileName, "Oddity");
			//var cyphersList = repo.LoadItems();
			//var cyphersList = repo.DeserializeXmlToList();
			ConsoleKeyInfo c = new ConsoleKeyInfo();
            while (c.Key != ConsoleKey.Escape)
            {
				Console.WriteLine("Press G to generate or ESC to exit");
				c = Console.ReadKey();
                if (c.KeyChar == 'g')
                {
                    //Console.WriteLine($"Generate from {cyphersList.Count}!");
                    //var rand = new Random((int)DateTime.UtcNow.Ticks);
                    var rand = new Random(Guid.NewGuid().GetHashCode());
                    var rndInt = rand.Next(cyphersList.Count);
                    Console.WriteLine($"1d6 = {rand.Next(1, 6)}");
                    Console.WriteLine(cyphersList[rndInt].ToString());
                }
				//c = Console.ReadKey();
            }
        }

		/// <summary>
		/// View all unique attributes to create specific class
		/// </summary>
		/// <param name="xmlFile"></param>
		/// <param name="element"></param>
		private static void ViewUniqueAttributes(string xmlFile, string element)
		{
			XDocument doc = XDocument.Load(xmlFile);
			var uniqueElements = new List<string>();
			foreach(var elem in doc.Elements().First().Elements(element))
			{
				// elements() returns direct children
				// descendants() recurses
				foreach(var attr in elem.Elements())
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
