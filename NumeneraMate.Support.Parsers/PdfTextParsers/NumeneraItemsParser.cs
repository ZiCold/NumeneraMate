using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NumeneraMate.Support.Parsers.PdfTextParsers
{
    public static class NumeneraItemsParser
	{
		/// <summary>
		/// Parse file with Numenera items to XML
		/// </summary>
		/// <param name="itemsFileName"></param>
		/// <param name="keywordsFileName"></param>
		/// <param name="sourcebookName"></param>
		/// <param name="itemsNames"></param>
		/// <param name="itemName"></param>
		public static void NumeneraItemParseFileToXML(string itemsFileName, string keywordsFileName, string sourcebookName, string itemsNames, string itemName)
		{
			var info = new PDFTextFileInfo()
			{
				ItemsFileName = itemsFileName,
				TableKeyword = null
			};
			PDFTextParser.LoadKeywordsFromFile(info, keywordsFileName);
			NumeneraCreateXML(info, sourcebookName, itemsNames, itemName);
		}

		/// <summary>
		/// Parses file with Cyphers copied from PDF
		/// </summary>
		/// <param name="sourcebookName"></param>
		/// <param name="itemsFileName"></param>
		public static void CyphersParseFileToXML(string itemsFileName, string sourcebookName)
		{
			var info = new PDFTextFileInfo()
			{
				ItemsFileName = itemsFileName,
				KeywordsList = new List<string>()
				{
					"Level:", "Internal:", "Wearable:", "Usable:", "Effect:", "Table:"
				},
				TableKeyword = "Table:"
			};

			NumeneraCreateXML(info, sourcebookName, "Cyphers", "Cypher");
		}


		/// <summary>
		/// Parses file with Artefacts copied from PDF
		/// </summary>
		/// <param name="sourcebookName"></param>
		/// <param name="itemsFileName"></param>
		public static void ArtefactsParseFileToXML(string itemsFileName, string sourcebookName)
		{
			var info = new PDFTextFileInfo()
			{
				ItemsFileName = itemsFileName,
				KeywordsList = new List<string>()
				{
					"Level:", "Form:", "Effect:", "Table:", "Depletion:"
				},
				TableKeyword = "Table:"
			};

			NumeneraCreateXML(info, sourcebookName, "Artefacts", "Artefact");
		}

		/// <summary>
		/// Create XML from file with oddities
		/// First digits are passed by (roll numbers)
		/// </summary>
		/// <param name="source"></param>
		/// <param name="itemsFileName"></param>
		public static void OdditiesParseFileToXML(string source, string itemsFileName)
		{
			var strings = PDFTextParser.ParseFileWithRollTableToStrings(itemsFileName);

			var xmlInfo = new PDFTextXmlInfo()
			{
				XmlFileName = Path.GetDirectoryName(itemsFileName) +
							@"\OUTPUT_Oddities_" + source + ".xml",
				ObjectsName = "Oddities",
				ObjectName = "Oddity",
				Source = "Discovery"
			};
			PDFTextXmlCreator.CreateXMLForStrings(xmlInfo, strings);
		}

		/// <summary>
		/// Takes file with MinCraftingLevel and names
		/// And inserts levels to XML file
		/// </summary>
		/// <param name="dir"></param>
		/// <param name="numeneraType"></param>
		/// <param name="source"></param>
		public static void AddMinCraftLvlToXML(string dir, string numeneraType, string source)
		{
			string singleNumenera = numeneraType.Substring(0, numeneraType.Length - 1);

			// change xml file
			string xmlFile = dir + @"\" + numeneraType + @"\OUTPUT_" +
				numeneraType + "_" + source;
			XDocument doc;
			try
			{
				doc = XDocument.Load(xmlFile + ".xml");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}
			var currentObjects = doc.Element(numeneraType).Descendants(singleNumenera);
			string temp = currentObjects.FirstOrDefault().Element("Name").Value;
			var smth = from xe in currentObjects
					   where xe.Element("Name").Value.ToLower() == "armored ﬂesh"
					   select xe;


			// parse file with data
			string dataFile = dir + @"\" + numeneraType + @"\MinCraftingLevel_" +
				numeneraType + "_" + source + ".txt";
			string[] lines = File.ReadAllLines(dataFile);
			var parsedLines = new Dictionary<string, string>();

			int updatedObjectsCount = 0;
			var notUpdated = new List<string>();
			for (int i = 0; i < lines.Length; i++)
			{
				var spaceIndex = lines[i].IndexOf(' ');
				var numeneraLevel = lines[i].Substring(0, spaceIndex);
				var numeneraName = lines[i].Substring(spaceIndex + 1, lines[i].Length - spaceIndex - 1).ToLower();

				var rightElement = from xe in currentObjects
								   where (string)xe.Element("Name").Value.ToLower() == numeneraName
								   select xe;
				if (rightElement.FirstOrDefault() != null)
				{
					rightElement.FirstOrDefault()
						.Element("Level")
						.AddAfterSelf(new XElement("MinimumCraftingLevel", numeneraLevel));
					updatedObjectsCount++;
				}
				else
				{
					notUpdated.Add(numeneraName);
				}
			}

			doc.Save(xmlFile + "_updated.xml");
			Console.WriteLine($"Elements in XML: {currentObjects.Count()}\nElements in file: {lines.Length}\nUpdated objects: {updatedObjectsCount}");
			Console.WriteLine("\nNot Updated Elements:");
			notUpdated.ForEach(c => Console.WriteLine(c));
		}

		/// <summary>
		/// Creates XML file from Numenera items extracted from PDF
		/// </summary>
		/// <param name="info"></param>
		/// <param name="sourcebookName"></param>
		/// <param name="objectsName"></param>
		/// <param name="objectName"></param>
		private static void NumeneraCreateXML(PDFTextFileInfo info, string sourcebookName, string objectsName, string objectName)
		{
			var linesFromFile = File.ReadAllLines(info.ItemsFileName);
			var itemsLines = new List<string>(linesFromFile);
			itemsLines = PDFTextParser.FixBrokenWords(itemsLines);
			itemsLines = PDFTextParser.SplitLinesByKeywords(itemsLines, info.KeywordsList);
			var objects = PDFTextParser.SplitItemsToObjects(info, itemsLines.ToArray());

			var smthToWrite = new List<string>();
			objects.ForEach(x => x.ForEach(s => smthToWrite.Add(s)));
			File.WriteAllLines(info.ItemsFileName + "_Cleared.txt", smthToWrite);

			var parsedDic = PDFTextParser.CreateDictionariesFromObjects(objects, info.KeywordsList);

			// and make XML from it
			var xmlInfo = new PDFTextXmlInfo()
			{
				XmlFileName = Path.GetDirectoryName(info.ItemsFileName) +
							@"\OUTPUT_" + objectsName + "_" + sourcebookName + ".xml",
				ObjectsName = objectsName,
				ObjectName = objectName,
				Source = sourcebookName
			};

			PDFTextXmlCreator.CreateXMLWithRollTable(xmlInfo, parsedDic, info.TableKeyword);
		}
	}
}
