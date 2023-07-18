using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp
{
    public class CreateJsonFromFocusesExcel
    {
        public static void Generate()
        {
            var directory = @"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\characters_tables\excel files\";
            var filename = "Focuses.xlsx";
            var filepath = Path.Combine(directory, filename);

            XSSFWorkbook xssfwb;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }
            ISheet sheet = xssfwb.GetSheetAt(0);

            var firstRow = sheet.GetRow(0);

            var focusesList = new List<Focus>();
            for (int rowId = 1; rowId <= sheet.LastRowNum; rowId++) // !!! sheet.LastRowNum
            {
                var currentRow = sheet.GetRow(rowId);
                if (currentRow == null) continue;   // pass row with empty cells

                var focus = new Focus();

                focus.Source = currentRow.GetCell(0).StringCellValue;
                focus.Name = currentRow.GetCell(1).StringCellValue;

                // flavor splitted
                focus.Flavor = currentRow.GetCell(2).StringCellValue.Replace($"\r\n\r\n", "<br><br>").Replace("\n\n", "<br><br>").Replace("\n", "<br><br>");

                focus.Connection = GetAbilitiesList(currentRow.GetCell(3));
                focus.Additional = GetAbilitiesList(currentRow.GetCell(4));
                focus.MinorMajorEffects = GetAbilitiesList(currentRow.GetCell(5));
                focus.Tier1 = GetAbilitiesList(currentRow.GetCell(6));
                focus.Tier2 = GetAbilitiesList(currentRow.GetCell(7));
                focus.Tier3ChooseOne = GetAbilitiesList(currentRow.GetCell(8));
                focus.Tier4 = GetAbilitiesList(currentRow.GetCell(9));
                focus.Tier5 = GetAbilitiesList(currentRow.GetCell(10));
                focus.Tier6ChooseOne = GetAbilitiesList(currentRow.GetCell(11));


                // get cell values
                /*for (int columnId = 0; columnId < firstRow.LastCellNum; columnId++)
                {
                    var currentCell = currentRow.GetCell(columnId);
                    var cellValue = "";
                    if (currentCell != null)
                    {
                        cellValue = currentRow.GetCell(columnId).StringCellValue;
                    }
                }*/
                focusesList.Add(focus);
            }

            string output = JsonConvert.SerializeObject(focusesList,
                Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            File.WriteAllText(@"C:\Users\ZiCold\YandexDisk\TRPGs - Numenera\zicold.github.io\characters_tables\focuses\focuses.json", output);
            //Console.WriteLine(output);
        }

        public static List<Ability> GetAbilitiesList(ICell cell)
        {
            var result = new List<Ability>();
            if (cell == null) return result;

            var split = cell.StringCellValue.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

            foreach (var line in split)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var colonIndex = line.IndexOf(':');
                if (colonIndex == -1)
                {
                    result.Add(new Ability() { Description = line });
                }
                else
                {
                    var ability = new Ability()
                    {
                        Description = line.Substring(colonIndex + 1, line.Length - colonIndex - 1).Trim()
                    };

                    var abilityName = line.Substring(0, colonIndex);

                    var bracket = abilityName.IndexOf('(');
                    if (bracket > 0)
                    {
                        abilityName = line.Substring(0, bracket - 1);
                        var cost = line[bracket + 1].ToString();
                        if (char.IsDigit(line[bracket + 2]) || line[bracket + 2] == '+') cost += line[bracket + 2];
                        if (char.IsDigit(line[bracket + 3]) || line[bracket + 3] == '+') cost += line[bracket + 3];
                        if (char.IsDigit(line[bracket + 4]) || line[bracket + 4] == '+') cost += line[bracket + 4];

                        var forPoolFind = line.Substring(bracket, colonIndex);
                        var forPoolSplitted = forPoolFind.Split(' ');
                        var pool = forPoolSplitted[1];
                        ability.Cost = cost;
                        ability.Pool = pool;
                    }
                    ability.Name = abilityName;
                    result.Add(ability);
                }

            }
            return result;
        }
    }
}
