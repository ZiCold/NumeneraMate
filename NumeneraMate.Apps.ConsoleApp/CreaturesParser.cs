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
        public static List<Creature> GetCreaturesListFromExcel(string directory, string filename)
        {
            var filepath = Path.Combine(directory, filename);

            XSSFWorkbook xssfwb;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            xssfwb.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;

            var sheetTitle = "Creatures";

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

            for (int i = 1; i <= 11; i++)
            {
                var currentCell = currentRow.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                var isItInTerrain = false;
                if (currentCell != null)
                {
                    if (currentCell.CellType != CellType.Numeric && !string.IsNullOrEmpty(currentCell.StringCellValue.Trim()))
                        isItInTerrain = true;
                }
                    
                switch (i)
                {
                    case 1:
                        oneCreature.Name = currentCell.StringCellValue; break;
                    case 2:
                        oneCreature.Source = currentCell.StringCellValue; break;
                    case 3:
                        var endless = currentCell.NumericCellValue.ToString();
                        oneCreature.UsedInEndlessLegendCampaign = !string.IsNullOrEmpty(endless) && endless != "0";
                        break;
                    case 4:
                        oneCreature.RuinsUnderground = isItInTerrain; break;
                    case 5:
                        oneCreature.PlainsHills = isItInTerrain; break;
                    case 6:
                        oneCreature.Desert = isItInTerrain; break;
                    case 7:
                        oneCreature.Woods = isItInTerrain; break;
                    case 8:
                        oneCreature.Mountains = isItInTerrain; break;
                    case 9:
                        oneCreature.Swamp = isItInTerrain; break;
                    case 10:
                        oneCreature.Dimensions = isItInTerrain; break;
                    case 11:
                        oneCreature.Water = isItInTerrain; break;
                }
            }

            return oneCreature;
        }
    }
}
