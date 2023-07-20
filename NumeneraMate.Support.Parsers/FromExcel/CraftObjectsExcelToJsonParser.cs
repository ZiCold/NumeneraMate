using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumeneraMate.Libs.NumeneraObjects.Craft;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System.Data.Common;
using System.Text.Json;

namespace NumeneraMate.Support.Parsers.FromExcel
{
    public class CraftObjectsExcelToJsonParser : JsonParser
    {
        public CraftObjectsExcelToJsonParser()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public void ParseExcelToJson(string craftingExcelPath, string jsonFilePath)
        {
            var list = GetCraftObjectsList(craftingExcelPath);
            var json = JsonSerializer.Serialize(list, GetJsonOptions());
            File.WriteAllText(jsonFilePath, json);
        }

        public List<CraftObjectJson> GetCraftObjectsList(string excelFileName)
        {
            var craftObjectsList = new List<CraftObjectJson>();
            using (var excel = new ExcelPackage(excelFileName))
            {
                var sheetIotum = excel.Workbook.Worksheets["Craft Objects Iotum"];
                craftObjectsList = GetIotumCostList(sheetIotum);

                AddSpecs(excel.Workbook.Worksheets["Installations"], craftObjectsList);

                AddSpecs(excel.Workbook.Worksheets["Automatons"], craftObjectsList);

                AddSpecs(excel.Workbook.Worksheets["Vehicles"], craftObjectsList);
            }
            craftObjectsList.ForEach(x => x.Name = UppercaseFirstLetters(x.Name));
            return craftObjectsList;
        }

        private void AddSpecs(ExcelWorksheet sheet, List<CraftObjectJson> craftObjectsList)
        {
            for (int i = 2; i < sheet.Dimension.Rows; i++)
            {
                var craftObject = craftObjectsList.First(x => x.Name == sheet.Cells[i, 2].Text.Trim());
                craftObject.Specifications = sheet.Cells[i, 4].Text;
                craftObject.Modifications = sheet.Cells[i, 5].Text;
                craftObject.Depletion = sheet.Cells[i, 6].Text;
            }
        }

        private List<CraftObjectJson> GetIotumCostList(ExcelWorksheet sheet)
        {
            var craftObjectsList = new List<CraftObjectJson>();
            for (int i = 2; i < sheet.Dimension.Rows; i++)
            {
                if (string.IsNullOrEmpty(sheet.Cells[i, 1].Text)) break;
                var craftObj = new CraftObjectJson()
                {
                    Kind = sheet.Cells[i, 1].Text,
                    Name = sheet.Cells[i, 2].Text.Trim(),
                    MinCraftingLevel = GetCellIntValue(3),
                    Parts = GetCellIntValue(4),
                    Iotum = new IotumJson()
                    {
                        Io = GetCellIntValue(5),
                        ResponsiveSynth = GetCellIntValue(6),
                        AptClay = GetCellIntValue(7),
                        PliableMetal = GetCellIntValue(8),
                        MimeticGel = GetCellIntValue(9),
                        AmberCrystal = GetCellIntValue(10),
                        Psiranium = GetCellIntValue(11),
                        Oraculum = GetCellIntValue(12),
                        TamedIron = GetCellIntValue(13),
                        CosmicFoam = GetCellIntValue(14)
                    }
                };
                int GetCellIntValue(int column)
                {
                    return string.IsNullOrEmpty(sheet.Cells[i, column].Text) ? 0 : int.Parse(sheet.Cells[i, column].Text);
                }

                craftObjectsList.Add(craftObj);
            }
            return craftObjectsList;
        }

        public string UppercaseFirstLetters(string input)
        {
            char[] array = input.ToLower().ToCharArray();
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
    }
}
