using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NumeneraMate.Apps.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
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
                Console.WriteLine($"{cyphersFile} cyphers count: {cyphersList.Count}");
				//cyphersList.ForEach(x => System.Console.WriteLine(x.ToString()));
            }


			//ViewUniqueAttributes(dir + fileName, "Oddity");
			//var cyphersList = repo.LoadItems();
			//var cyphersList = repo.DeserializeXmlToList();

			//Console.WriteLine("Press G to generate or ESC to exit");
			//var c = Console.ReadKey();
			//while (c.Key != ConsoleKey.Escape)
			//{
			//	if (c.KeyChar == 'g')
			//	{
			//		Console.WriteLine($"Generate from {cyphersList.Count}!");
			//		//var rand = new Random((int)DateTime.UtcNow.Ticks);
			//		var rand = new Random(Guid.NewGuid().GetHashCode());
			//		var rndInt = rand.Next(cyphersList.Count);
			//		Console.WriteLine($"1d6 = {rand.Next(1, 6)}");
			//		Console.WriteLine(cyphersList[rndInt].ToString());
			//	}
			//	c = Console.ReadKey();
			//}
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
