using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NumeneraMate.Libs.Creatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp
{
    public static class CreaturesParser
    {
        public static List<Creature> GetCreaturesListFromExcel()
        {
            var excelTableDirectory = @"E:\Documents\Tabletop RPGs\Numenera\APPs\Creatures\";
            var excelTableFilename = @"CreaturesTable.xlsx";

            var filepath = Path.Combine(excelTableDirectory, excelTableFilename);

            XSSFWorkbook xssfwb;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            var sheetTitle = "Creatures List";

            ISheet sheet = xssfwb.GetSheet(sheetTitle);

            var creaturesList = new List<Creature>();

            for (int rowId = 1; rowId <= sheet.LastRowNum; rowId++)
            {
                var currentRow = sheet.GetRow(rowId);
                if (currentRow == null) continue;

                var singleCreature = ProcessRow(currentRow);
                creaturesList.Add(singleCreature);
            }

            return creaturesList;
        }

        private static Creature ProcessRow(IRow currentRow)
        {
            var oneCreature = new Creature();
            oneCreature.Name = currentRow.GetCell(1).StringCellValue;
            oneCreature.Source = currentRow.GetCell(2).StringCellValue;

            var endless = currentRow.GetCell(3).NumericCellValue.ToString();
            oneCreature.UsedInEndlessLegendCampaign = !string.IsNullOrEmpty(endless) && endless != "0";

            var ruins = currentRow.GetCell(4).StringCellValue.Trim();
            oneCreature.RuinsUnderground = !string.IsNullOrEmpty(ruins);

            var plains = currentRow.GetCell(5).StringCellValue.Trim();
            oneCreature.PlainsHills = !string.IsNullOrEmpty(plains);

            var desert = currentRow.GetCell(6).StringCellValue.Trim();
            oneCreature.Desert = !string.IsNullOrEmpty(desert);

            var woods = currentRow.GetCell(7).StringCellValue.Trim();
            oneCreature.Woods = !string.IsNullOrEmpty(woods);

            var mountains = currentRow.GetCell(8).StringCellValue.Trim();
            oneCreature.Mountains = !string.IsNullOrEmpty(mountains);

            var swamp = currentRow.GetCell(9).StringCellValue.Trim();
            oneCreature.Swamp = !string.IsNullOrEmpty(swamp);

            var dimensions = currentRow.GetCell(10).StringCellValue.Trim();
            oneCreature.Dimensions = !string.IsNullOrEmpty(dimensions);

            var water = currentRow.GetCell(11).StringCellValue.Trim();
            oneCreature.Water = !string.IsNullOrEmpty(water);

            return oneCreature;
        }
    }
}
