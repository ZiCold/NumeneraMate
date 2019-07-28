using NumeneraMate.Libs.DataAccess;
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
			var fileName = "Cyphers_Discovery.xml";
			ViewUniqueAttributes(dir + fileName, "Cypher");
			var repo = new XMLRepo(dir + fileName);
			//var cyphersList = repo.LoadItems();
			//var cyphersList = repo.DeserializeXmlFile();
			var cyphersList = repo.DeserializeXmlToList();
			cyphersList.ForEach(x => System.Console.WriteLine(x.ToString()));
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
