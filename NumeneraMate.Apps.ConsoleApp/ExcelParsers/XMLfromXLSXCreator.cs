using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Xml.Linq;
using System.IO;

namespace NumeneraMate.Apps.ConsoleApp
{
    public class XMLfromXLSXCreator
    {
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
