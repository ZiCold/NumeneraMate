using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.Devices
{
	public class XMLRepo
	{
		private string _xmlFileName;

		public XMLRepo(string xmlFileName)
		{
			_xmlFileName = xmlFileName;
		}

		public List<Cypher> LoadItems()
		{
			XDocument xdoc = XDocument.Load(_xmlFileName);

			var smth = from c in xdoc.Descendants("Cyphers").Elements("Cypher")
					   select new Cypher()
					   {
						   Name = (string)c.Element("Name"),
						   Level = (string)c.Element("Level"),
						   MinimumCraftingLevel = (int)c.Element("MinimumCraftingLevel"),
						   Wearable = (string)c.Element("Wearable"),
						   Usable = (string)c.Element("Wearable"),
						   Internal = (string)c.Element("Internal"),
						   Effect = (string)c.Element("Effect")
					   };
			var cyphersList = smth.ToList();
			return cyphersList;
		}

		public NumeneraCyphers DeserializeXmlFile()
		{
			var cyphersXml = File.ReadAllText(_xmlFileName);
			var cyphersList = DeserializeObject<NumeneraCyphers>(cyphersXml);
			return cyphersList;
		}

		public List<Cypher> DeserializeXmlToList()
		{
			var cyphersXml = File.ReadAllText(_xmlFileName);
			var cyphersList = DeserializeObject<List<Cypher>>(cyphersXml);
			return cyphersList;
		}

		public static T DeserializeObject<T>(string xml)
		{
			var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("Cyphers"));
			using (var tr = new StringReader(xml))
			{
				return (T)serializer.Deserialize(tr);
			}
		}

	}
}
