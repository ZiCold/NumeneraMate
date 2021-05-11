using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NumeneraMate.Apps.ConsoleApp.PdfTextParsers
{
    public class PDFTextXmlCreator
	{
		public static void CreateXML(PDFTextXmlInfo info, List<Dictionary<string, string>> objList, string tableKeyword = null)
		{
			XDocument xdoc = new XDocument();
			// root element
			var objects = new XElement(info.ObjectsName);
			objects.Add(new XAttribute("source", info.Source));

			foreach (var obj in objList)
			{
				// current element
				var curObject = new XElement(info.ObjectName);
				objects.Add(curObject);
				// loop for lines in one object
				foreach (var element in obj)
				{
					// clear string for xmlKey
					// by the best practices is better to avoid -, _, .
					string xmlKey = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(element.Key.ToLower());
					xmlKey = new string(xmlKey.Where(c => char.IsLetterOrDigit(c)).ToArray());

					var feature = new XElement(xmlKey, element.Value);
					curObject.Add(feature);
					if (tableKeyword != null && element.Key == tableKeyword)
						CreateXMLTable(info, feature);
				}
			}

			// saving final document
			xdoc.Add(objects);
			xdoc.Save(info.XmlFileName);
		}

		public static void CreateXMLTable(PDFTextXmlInfo info, XElement tableString)
		{
			var rows = tableString.Value.Split('#');
			tableString.RemoveAll();
			foreach (var row in rows)
			{
				if (!string.IsNullOrEmpty(row))
					tableString.Add(new XElement("Row", row));
			}
		}

		public static void CreateXMLWithRollTable(PDFTextXmlInfo info, List<Dictionary<string, string>> objList, string tableKeyword = null)
		{
			XDocument xdoc = new XDocument();
			// root element
			var objects = new XElement(info.ObjectsName);
			if (!string.IsNullOrEmpty(info.Source))
				objects.Add(new XAttribute("source", info.Source));
			var count = 0;

			foreach (var obj in objList)
			{
				// current element
				var curObject = new XElement(info.ObjectName);
				objects.Add(curObject);
				// loop for lines in one object
				foreach (var element in obj)
				{
					// clear string for xmlKey
					// by the best practices is better to avoid -, _, .
					string xmlKey = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(element.Key.ToLower());
					xmlKey = new string(xmlKey.Where(c => char.IsLetterOrDigit(c)).ToArray());

					if (tableKeyword != null && element.Key == tableKeyword)
					{
						var feature = new XElement("Roll" + xmlKey, element.Value);
						curObject.Add(feature);
						CreateXMLTableWithRollCell(info, feature);
					}
					else
					{
						var feature = new XElement(xmlKey, element.Value);
						curObject.Add(feature);
					}
				}
				count++;
			}

			objects.Add(new XAttribute("quantity", count));
			// saving final document
			xdoc.Add(objects);
			try
			{
				xdoc.Save(info.XmlFileName);
			}
			catch (Exception ex)
			{
				ex.Data["UserMessage"] += "An error occured while trying to save XML file;";
				throw;
			}
		}

		public static void CreateXMLTableWithRollCell(PDFTextXmlInfo info, XElement tableString)
		{
			var rows = tableString.Value.Split('#');
			tableString.RemoveAll();
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row)) continue;
				if (!char.IsDigit(row[0]))
				{
					tableString.Descendants().Last().Value += " " + row;
					continue;
				}

				var curRow = new XElement("Row", "");
				var spaceIndex = row.IndexOf(' ');
				var rollCell = row.Substring(0, spaceIndex);
				var resultCell = row.Substring(spaceIndex + 1, row.Length - spaceIndex - 1);

				curRow.Add(new XElement("Roll", rollCell));
				curRow.Add(new XElement("Result", resultCell));

				tableString.Add(curRow);
			}
		}

		/// <summary>
		/// Parse list of string to XML (for oddities)
		/// </summary>
		/// <param name="info"></param>
		/// <param name="objList"></param>
		public static void CreateXMLForStrings(PDFTextXmlInfo info, List<string> objList)
		{
			XDocument xdoc = new XDocument();
			var objects = new XElement(info.ObjectsName);
			if (!string.IsNullOrEmpty(info.Source))
				objects.Add(new XAttribute("source", info.Source));
			var count = 0;

			foreach (var obj in objList)
			{
				if (!string.IsNullOrEmpty(obj))
				{
					objects.Add(new XElement(info.ObjectName, obj));
					count++;
				}
			}
			objects.Add(new XAttribute("quantity", count));
			xdoc.Add(objects);
			xdoc.Save(info.XmlFileName);
		}

	}
}
