using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NumeneraMate.Apps.ConsoleApp
{
    // Read .xlsx using NPOI -> fork of Java POI Library
    // https://github.com/dotnetcore/NPOI
    // there is version for .Net Core - https://github.com/dotnetcore/NPOI
    // http://codemonkeydeveloper.blogspot.com/2015/03/npoi.html
    public class HTMLTableFromXLSXCreator
    {
        public static void Transform()
        {
            //CreateHTMLTable_CraftingCosts();
            //CreateHTMLTable_CraftingDescriptions();


            var directory = @"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\NumeneraAppFiles\CharacterGeneration 2.5\";
            var outputDirectory = @"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\zicold.github.io\characters_tables\";
            //CreateHTMLTable_Descriptors();
            CreateHTMLTable_Focuses();
            var typesFileName = "TypesAbilities.xlsx";
            CreateHTMLTable_Type(Path.Combine(directory, typesFileName), outputDirectory, 4);
            //for (int i = 0; i < 6; i++) { CreateHTMLTable_Type(i); }
        }

        public static void CreateHTMLTable_CraftingDescriptions()
        {
            var directory = @"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\zicold.github.io\crafting\Excel Files\";
            var filename = "CraftingTables.xlsx";
            var filepath = Path.Combine(directory, filename);
            XSSFWorkbook xssfwb;

            // open .xlsx file
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            var sheetTitle = "Installations";
            ISheet sheet = xssfwb.GetSheet(sheetTitle);
            var htmlHeader = GetHtmlHeader(sheetTitle, "Numenera " + sheetTitle);
            var tableBegin = GetTableBeginning();

            // header row
            var firstRow = sheet.GetRow(0);
            var htmlRowHeader = "<tr>";
            for (int colId = 0; colId < firstRow.LastCellNum; colId++)
            {
                var cellValue = firstRow.GetCell(colId).StringCellValue;
                var curHeaderRow = Environment.NewLine + "<th ";
                curHeaderRow += GetStyleForThCell_ForInstallations(cellValue);

                curHeaderRow += ">" + cellValue + "</th>";
                htmlRowHeader += curHeaderRow;
            }
            htmlRowHeader += Environment.NewLine + "</tr>";

            // rows
            sheet = xssfwb.GetSheet("Automatons");
            var htmlRowsAutomatons = GetStringFromXLSXrows_ForCrafting(sheet, firstRow);
            sheet = xssfwb.GetSheet("Installations");
            var htmlRowsInstallations = GetStringFromXLSXrows_ForCrafting(sheet, firstRow);
            sheet = xssfwb.GetSheet("Vehicles");
            var htmlRowsVehicles = GetStringFromXLSXrows_ForCrafting(sheet, firstRow);

            var bodyEnd = "</body></html>";
            var tableEnd = "</table>";
            // combine everything
            var finalHtml = htmlHeader +
                tableBegin + Environment.NewLine +
                htmlRowHeader + Environment.NewLine +
                htmlRowsAutomatons + Environment.NewLine +
                htmlRowsInstallations + Environment.NewLine +
                htmlRowsVehicles + Environment.NewLine +
                tableEnd + Environment.NewLine +
                bodyEnd;

            var outputDirectory = @"E:\Projects\Github\zicold.github.io\crafting_tables\";
            outputDirectory = @"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\zicold.github.io\crafting\Excel Files\";
            File.WriteAllText(outputDirectory + Path.GetFileNameWithoutExtension(filename) + $"_{sheetTitle}.html", finalHtml);
        }

        public static void CreateHTMLTable_CraftingCosts()
        {
            var directory = @"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\zicold.github.io\crafting\Excel Files\";
            var filename = "Endless Legend - Crafting Tables.xlsx";
            var filepath = Path.Combine(directory, filename);
            XSSFWorkbook xssfwb;

            // open .xlsx file
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            var sheetTitle = "Craft Objects Iotum";
            ISheet sheet = xssfwb.GetSheet(sheetTitle);
            var htmlHeader = GetHtmlHeader(sheetTitle, "Numenera " + sheetTitle);
            var tableBegin = GetTableBeginning();

            // header row
            var firstRow = sheet.GetRow(0);
            var htmlRowHeader = "<tr>";
            for (int colId = 0; colId < firstRow.LastCellNum; colId++)
            {
                var cellValue = firstRow.GetCell(colId).StringCellValue;
                var curHeaderRow = Environment.NewLine + "<th ";
                curHeaderRow += GetStyleForThCell_ForInstallations(cellValue);

                curHeaderRow += ">" + cellValue + "</th>";
                htmlRowHeader += curHeaderRow;
            }
            htmlRowHeader += Environment.NewLine + "</tr>";

            // rows
            var htmlRows = GetStringFromXLSXrows_ForCrafting(sheet, firstRow);

            var bodyEnd = "</body></html>";
            var tableEnd = "</table>";
            // combine everything
            var finalHtml = htmlHeader +
                tableBegin + Environment.NewLine +
                htmlRowHeader + Environment.NewLine +
                htmlRows + Environment.NewLine +
                tableEnd + Environment.NewLine +
                bodyEnd;

            var outputDirectory = @"E:\Projects\Github\zicold.github.io\crafting_tables\";
            outputDirectory = directory;
            File.WriteAllText(outputDirectory + Path.GetFileNameWithoutExtension(filename) + $"_{sheetTitle}.html", finalHtml);
        }


        private static string GetStringFromXLSXrows_ForCrafting(ISheet sheet, IRow firstRow)
        {
            var htmlRows = "";
            for (int rowId = 1; rowId <= sheet.LastRowNum; rowId++)
            {
                var currentRow = sheet.GetRow(rowId);
                if (currentRow == null) continue;   // pass row with empty cells

                var htmlRow = "<tr>";

                // get cell values
                for (int columnId = 0; columnId < firstRow.LastCellNum; columnId++)
                {
                    var currentCell = currentRow.GetCell(columnId);
                    var cellValue = "";
                    if (currentCell != null)
                    {
                        if (currentCell.CellType == CellType.Numeric)
                            cellValue = currentCell.NumericCellValue.ToString();
                        else
                            cellValue = currentCell.StringCellValue;

                        string linkToPage;
                        if (firstRow.GetCell(3).StringCellValue == "Specifications")
                            linkToPage = "CraftingTables_CraftingCosts.html";
                        else
                            linkToPage = "CraftingTables_Installations.html";
                        switch (firstRow.GetCell(columnId).StringCellValue.ToLower())
                        {
                            case "name":
                                var refId = cellValue.Trim().ToLower().Replace(",", "").Replace(" ", "-");
                                htmlRow += Environment.NewLine + "<td id=\"" + refId + "\">"
                                        + $"<a href=\"{linkToPage}#{refId}\" target=\"_blank\">" + cellValue + "</a>"
                                        + "</td>";
                                break;
                            case "minlvl":
                                htmlRow += Environment.NewLine + "<td style=\"text-align:center\">" + cellValue + "</td>"; break;
                            default:
                                htmlRow += Environment.NewLine + "<td>" + cellValue + "</td>"; break;
                        }
                    }
                    else
                    {
                        htmlRow += Environment.NewLine + "<td>" + cellValue + "</td>";
                    }
                }
                htmlRow += Environment.NewLine + "</tr>";
                htmlRows += htmlRow;
            }
            return htmlRows;
        }

        private static string GetStyleForThCell_ForInstallations(string cellValue)
        {
            switch (cellValue)
            {
                case "Specifications":
                    return "style= \"width: 900px\"";
                case "Modifications":
                    return "style= \"width: 300px\"";
                default:
                    return "";
            }
        }

        #region CharactersTables
        public static void CreateHTMLTable_Type(string pathToFile, string outputDirectory, int bookPage)
        {
            XSSFWorkbook xssfwb;

            // open .xlsx file
            using (FileStream file = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            // get first sheet
            ISheet sheet = xssfwb.GetSheetAt(bookPage);
            var typeName = sheet.SheetName;
            Log(typeName + " sheet found");
            var title = typeName + " Abilities";

            var htmlHeader = GetHtmlHeader(title);
            var tableBegin = GetTableBeginning("min-width: 3500px");

            // header row
            var firstRow = sheet.GetRow(0);
            Log("There " + firstRow.LastCellNum + " columns");
            var htmlRowHeader = "<tr>";
            for (int colId = 0; colId < firstRow.LastCellNum; colId++)
            {
                var cellValue = firstRow.GetCell(colId).StringCellValue;
                var curHeaderRow = Environment.NewLine + "<th ";
                curHeaderRow += GetStyleForThCell_TypesAbilities(cellValue);

                curHeaderRow += ">" + cellValue + "</th>";
                htmlRowHeader += curHeaderRow;
            }
            htmlRowHeader += Environment.NewLine + "</tr>";
            Log("Header row processed");

            // and now rows itself
            var htmlRows = GetStringFromXLSXrows_ForCharacters(sheet, firstRow);

            var tableEnd = "</table>";
            var bodyEnd = "</body></html>";
            // combine everything
            var finalHtnl = htmlHeader + Environment.NewLine +
                tableBegin + Environment.NewLine +
                htmlRowHeader + Environment.NewLine +
                htmlRows + Environment.NewLine +
                tableEnd + Environment.NewLine + bodyEnd;

            File.WriteAllText(outputDirectory + Path.GetFileNameWithoutExtension(pathToFile) + "_Table_" + typeName + ".html", finalHtnl);
        }

        private static string GetStyleForThCell_TypesAbilities(string cellValue)
        {
            switch (cellValue)
            {
                case "Type":
                    return "style= \"width: 60px\"";
                case "Tier 1":
                    return "style= \"width: 500px\"";
                default:
                    return "style= \"width: 400px\"";
            }
        }

        public static void CreateHTMLTable_Focuses()
        {
            var directory = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\characters_tables\excel files\";
            var filename = "Focuses.xlsx";
            var filepath = Path.Combine(directory, filename);
            var title = "Focuses";

            var htmlHeader = GetHtmlHeader(title);

            var tableBegin = GetTableBeginning("min-width: 4500px");

            XSSFWorkbook xssfwb;

            // open .xlsx file
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            // get first sheet
            ISheet sheet = xssfwb.GetSheetAt(0);

            // header row
            var firstRow = sheet.GetRow(0);
            var htmlRowHeader = "<tr>";
            for (int colId = 0; colId < firstRow.LastCellNum; colId++)
            {
                var cellValue = firstRow.GetCell(colId).StringCellValue;
                var curHeaderRow = Environment.NewLine + "<th ";
                curHeaderRow += GetStyleForThCell_Focuses(cellValue);

                curHeaderRow += ">" + cellValue + "</th>";
                htmlRowHeader += curHeaderRow;
            }
            htmlRowHeader += Environment.NewLine + "</tr>";

            // and now rows itself
            var htmlRows = GetStringFromXLSXrows_ForCharacters(sheet, firstRow);

            var tableEnd = "</table>";
            var bodyEnd = "</body></html>";
            // combine everything
            var finalHtnl = htmlHeader + Environment.NewLine +
                tableBegin + Environment.NewLine +
                htmlRowHeader + Environment.NewLine +
                htmlRows + Environment.NewLine +
                tableEnd + Environment.NewLine + bodyEnd;

            var outputDirectory = @"E:\Projects\Github\zicold.github.io\characters_tables\";
            File.WriteAllText(outputDirectory + Path.GetFileNameWithoutExtension(filename) + "_Table.html", finalHtnl);
        }

        private static string GetStyleForThCell_Focuses(string cellValue)
        {
            switch (cellValue)
            {
                case "Source":
                    return "style= \"width: 60px\"";
                case "Name":
                    return "style= \"width: 100px\"";
                case "Connection":
                case "Minor-MajorEffects":
                    return "style= \"width: 300px\"";
                default:
                    return "style= \"width: 400px\"";
            }
        }

        public static void CreateHTMLTable_Descriptors()
        {
            var directory = @"C:\Users\ZiCold\OneDrive\Numenera\CharacterGeneration 2.5\";
            var filename = "Descriptors.xlsx";
            var filepath = Path.Combine(directory, filename);
            var title = "Descriptors";

            var htmlHeader = GetHtmlHeader(title);

            var tableBegin = GetTableBeginning();

            XSSFWorkbook xssfwb;

            // open .xlsx file
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            // get first sheet
            ISheet sheet = xssfwb.GetSheetAt(0);

            // elemnts names
            var firstRow = sheet.GetRow(0);
            var htmlRowHeader = "<tr>";
            for (int colId = 0; colId < firstRow.LastCellNum; colId++)
            {
                var cellValue = firstRow.GetCell(colId).StringCellValue;
                var curHeaderRow = Environment.NewLine + "<th ";
                curHeaderRow += GetStyleForThCell_Descriptors(cellValue);
                curHeaderRow += ">" + cellValue + "</th>";
                htmlRowHeader += curHeaderRow;
            }
            htmlRowHeader += Environment.NewLine + "</tr>";

            // and now rows itself
            var htmlRows = GetStringFromXLSXrows_ForCharacters(sheet, firstRow);

            var bodyEnd = "</body></html>";
            var tableEnd = "</table>";
            // combine everything
            var finalHtnl = htmlHeader +
                tableBegin + Environment.NewLine +
                htmlRowHeader + Environment.NewLine +
                htmlRows + Environment.NewLine +
                tableEnd + Environment.NewLine +
                bodyEnd;

            var outputDirectory = @"E:\Projects\Github\zicold.github.io\characters_tables\";
            File.WriteAllText(outputDirectory + Path.GetFileNameWithoutExtension(filename) + "_Table.html", finalHtnl);
        }

        private static string GetStyleForThCell_Descriptors(string cellValue)
        {
            switch (cellValue)
            {
                case "Flavor":
                    return "style= \"width: 400px\"";
                case "Pools":
                    return "style= \"width: 140px\"";
                case "Other":
                    return "style= \"width: 300px\"";
                default:
                    return "";
            }
        }

        private static string GetStringFromXLSXrows_ForCharacters(ISheet sheet, IRow firstRow)
        {
            var htmlRows = "";
            for (int rowId = 1; rowId <= sheet.LastRowNum; rowId++)
            {
                var currentRow = sheet.GetRow(rowId);
                if (currentRow == null) continue;   // pass row with empty cells

                var htmlRow = "<tr>";

                // get cell values
                for (int columnId = 0; columnId < firstRow.LastCellNum; columnId++)
                {
                    var currentCell = currentRow.GetCell(columnId);
                    var cellValue = "";
                    if (currentCell != null)
                    {
                        cellValue = currentRow.GetCell(columnId).StringCellValue;

                        // bold ability names
                        var lineBreak = "<br><br>";
                        var boldCellValue = "";
                        if (cellValue.Contains(":"))
                        {
                            var splitted = cellValue.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);
                            foreach (var line in splitted)
                            {
                                // bold ability name
                                var colonIndex = line.IndexOf(':');
                                if (colonIndex > 0)
                                {
                                    var abilityName = "<b>" + line.Substring(0, colonIndex + 1) + "</b>";
                                    boldCellValue += abilityName + line.Substring(colonIndex + 1, line.Length - colonIndex - 1);
                                    boldCellValue += "<br>";
                                }
                                else
                                {
                                    boldCellValue += line + "<br>";
                                }
                            }
                        }

                        // convert line breaks
                        if (!string.IsNullOrEmpty(boldCellValue))
                            cellValue = boldCellValue;
                        else
                            cellValue = cellValue.Replace($"\r\n\r\n", lineBreak).Replace("\n\n", lineBreak);
                    }
                    htmlRow += Environment.NewLine + "<td>" + cellValue + Environment.NewLine + "</td>";
                }
                htmlRow += Environment.NewLine + "</tr>";
                htmlRows += htmlRow;

                Log("Row #" + rowId + " processed");
            }
            return htmlRows;
        }

        #endregion CharactersTables

        private static string GetHtmlHeader(string title, string customBodyHeader = "")
        {
            var head = "<!DOCTYPE html>" +
                "<html lang=\"en\">" +
                    "<head>" +
                    "<meta charset = \"utf-8\">" +
                    //"<meta name = \"viewport\" content = \"width=device-width, initial-scale=1\">" +
                    "<title>" + title + "</title>" +
                    "</head>";
            var bodyBegin = "<body>";
            var bodyHeader = "<header><h1>Numenera Character " + title + "</h1></header>";
            if (!string.IsNullOrEmpty(customBodyHeader))
                bodyHeader = "<header><h1>" + title + "</h1></header>";

            var htmlHeader = head +
                bodyBegin +
                bodyHeader;
            return htmlHeader;
        }

        private static string GetTableBeginning(string tableStyle = "")
        {
            return "<table " +
                "border=\"1\" cellspacing=\"0\" cellpadding=\"4px\" style=\"" + tableStyle + "\"> ";
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
