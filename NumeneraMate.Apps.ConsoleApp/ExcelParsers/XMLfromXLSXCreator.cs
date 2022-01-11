using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Xml.Linq;
using System.IO;
using NumeneraMate.Apps.ConsoleApp.XmlModels;

namespace NumeneraMate.Apps.ConsoleApp
{
    public static class XMLfromXLSXCreator
    {
        public static void CreateXMLsFromEventsTable(string eventsDirectory, string eventsXlsxName)
        {
            //CreateCreaturesXML(eventsDirectory, eventsXlsxName);
            CreateEncountersXML(eventsDirectory, eventsXlsxName);
        }

        private static void CreateEncountersXML(string directory, string xlsxName)
        {
            var encounters = new List<Encounter>();
            XSSFWorkbook xssfwb;
            using (FileStream file = new FileStream(Path.Combine(directory, xlsxName), FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            xssfwb.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;

            var sheetTitle = "Encounters";
            ISheet sheet = xssfwb.GetSheet(sheetTitle);

            for (int rowId = 1; rowId <= sheet.LastRowNum; rowId++)
            {
                var currentRow = sheet.GetRow(rowId);
                if (currentRow == null) continue;

                var singleEncounter = ProcessRowEncounter(currentRow);
                singleEncounter.Id = rowId;
                encounters.Add(singleEncounter);
            }

            var xdoc = new XDocument();
            var elements = new XElement("Encounters");
            foreach (var encounter in encounters)
            {
                var curElem = new XElement("Encounter");
                curElem.Add(new XElement(nameof(encounter.Id), encounter.Id));
                curElem.Add(new XElement(nameof(encounter.Description), encounter.Description));
                curElem.Add(new XElement(nameof(encounter.PlainsHills), encounter.PlainsHills));
                curElem.Add(new XElement(nameof(encounter.Desert), encounter.Desert));
                curElem.Add(new XElement(nameof(encounter.Woods), encounter.Woods));
                curElem.Add(new XElement(nameof(encounter.Mountains), encounter.Mountains));
                curElem.Add(new XElement(nameof(encounter.Swamp), encounter.Swamp));
                elements.Add(curElem);
            }
            xdoc.Add(elements);
            xdoc.Save(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\zicold.github.io\events_system\encounters.xml");

            return;
        }

        private static Encounter ProcessRowEncounter(IRow currentRow)
        {
            var encounter = new Encounter();
            for (int colNumber = 1; colNumber <= 6; colNumber++)
            {
                var currentCell = currentRow.GetCell(colNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                var isItInTerrain = false;
                if (currentCell != null)
                {
                    if (currentCell.CellType != CellType.Numeric && !string.IsNullOrEmpty(currentCell.StringCellValue.Trim()))
                        isItInTerrain = true;
                }

                switch (colNumber)
                {
                    case 1:
                        encounter.Description = currentCell.StringCellValue; break;
                    case 2:
                        encounter.PlainsHills = isItInTerrain; break;
                    case 3:
                        encounter.Desert = isItInTerrain; break;
                    case 4:
                        encounter.Woods = isItInTerrain; break;
                    case 5:
                        encounter.Mountains = isItInTerrain; break;
                    case 6:
                        encounter.Swamp = isItInTerrain; break;
                }
            }
            return encounter;
        }

        public static void CreateCreaturesXML(string directory, string xlsxName)
        {
            var creatures = CreaturesParser.GetCreaturesListFromExcel(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\HexCampaign\", @"Creatures and Events Table.xlsx");
            var xdoc = new XDocument();
            var elements = new XElement("Creatures");
            foreach (var creature in creatures)
            {
                var curElem = new XElement("Creature");
                curElem.Add(new XElement(nameof(creature.Name), creature.Name));
                curElem.Add(new XElement(nameof(creature.Source), creature.Source));
                curElem.Add(new XElement(nameof(creature.UsedInEndlessLegendCampaign), creature.UsedInEndlessLegendCampaign));
                curElem.Add(new XElement(nameof(creature.RuinsUnderground), creature.RuinsUnderground));
                curElem.Add(new XElement(nameof(creature.PlainsHills), creature.PlainsHills));
                curElem.Add(new XElement(nameof(creature.Desert), creature.Desert));
                curElem.Add(new XElement(nameof(creature.Woods), creature.Woods));
                curElem.Add(new XElement(nameof(creature.Mountains), creature.Mountains));
                curElem.Add(new XElement(nameof(creature.Swamp), creature.Swamp));
                curElem.Add(new XElement(nameof(creature.Dimensions), creature.Dimensions));
                curElem.Add(new XElement(nameof(creature.Water), creature.Water));
                elements.Add(curElem);
            }
            xdoc.Add(elements);
            xdoc.Save(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\zicold.github.io\events_system\creatures.xml");
            return;
        }

        public static void TransformDescriptors()
        {
            var directory = @"C:\Users\ZiCold\OneDrive\Numenera\CharacterGeneration 2.5\";
            var filename = "Descriptors v2.5.xlsx";
            var filepath = Path.Combine(directory, filename);
            var rootElementName = "CharacterDescriptions";
            var elementName = "Descriptor";

            XSSFWorkbook xssfwb;

            // open .xlsx file
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            // get first sheet
            ISheet sheet = xssfwb.GetSheetAt(0);

            // createXML
            var xdoc = new XDocument();
            var rootElement = CreateXEelementsTreeForSimpleXLSX(sheet, rootElementName, elementName);
            xdoc.Add(rootElement);

            // save XML
            var xmlFilePath = directory + Path.GetFileNameWithoutExtension(filename) + "_XML.xml";
            xdoc.Save(xmlFilePath);

            // minify
            MinifyXML(xmlFilePath);
        }

        public static XElement CreateXEelementsTreeForSimpleXLSX(ISheet sheet, string rootElementName, string elementName)
        {
            // elemnts names
            var attributes = new List<string>();
            var firstRow = sheet.GetRow(0);
            for (int colId = 0; colId < firstRow.LastCellNum; colId++)
            {
                var cellValue = firstRow.GetCell(colId).StringCellValue.Replace(" ", "");
                attributes.Add(cellValue);
            }

            var root = new XElement(rootElementName);

            // and now elements itself
            for (int rowId = 1; rowId <= sheet.LastRowNum; rowId++)
            {
                var currentRow = sheet.GetRow(rowId);
                if (currentRow == null) continue;   // row with empty cells only

                var curRowElement = new XElement(elementName);

                // get cell values
                for (int columnId = 0; columnId < currentRow.LastCellNum; columnId++)
                {
                    var currentCell = currentRow.GetCell(columnId);
                    var stringCellValue = "";
                    if (currentCell != null)
                    {
                        stringCellValue = currentRow.GetCell(columnId).StringCellValue;
                        // remove double line breaks
                        stringCellValue = stringCellValue.Replace($"\r\n\r\n", $"\r\n").Replace("\n\n", "\n");
                        if (columnId == 1) stringCellValue = UppercaseWords(stringCellValue.ToLower());
                    }

                    var curCellElement = new XElement(attributes[columnId], stringCellValue);
                    curRowElement.Add(curCellElement);
                }
                root.Add(curRowElement);
            }
            return root;
        }

        public static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }

            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        // Transform XML into one string for JS file
        public static void MinifyXML(string xmlFilePath)
        {
            var originalText = File.ReadAllText(xmlFilePath);
            var minifiedText = originalText.Replace("\r", " ").Replace("\n", " ").Replace("\"", "\\\"");
            var directory = xmlFilePath.Replace(Path.GetFileName(xmlFilePath), "");
            var filenameNoExt = Path.GetFileNameWithoutExtension(xmlFilePath);
            var outputFilename = directory + filenameNoExt + "_minified" + Path.GetExtension(xmlFilePath);
            File.WriteAllText(outputFilename, minifiedText);
        }
    }
}
