using NumeneraMate.Libs.NumeneraObjects.Events;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Support.Parsers.FromExcel
{
    public class EventsExcelParser
    {
        public EventsExcelParser()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public List<Encounter> GetEncounterList(string excelFileName)
        {
            var encountersList = new List<Encounter>();
            using(var excel = new ExcelPackage(excelFileName))
            {
                var sheet = excel.Workbook.Worksheets["Encounters"];
                for(int i = 2; i < sheet.Dimension.Rows; i++)
                {
                    if (string.IsNullOrEmpty(sheet.Cells[i, 1].Text)) break;
                    var enc = new Encounter()
                    {
                        Id = int.Parse(sheet.Cells[i, 1].Text),
                        Description = sheet.Cells[i, 2].Text,
                        PlainsHills = sheet.Cells[i, 3].Text == "+",
                        Desert = sheet.Cells[i, 4].Text == "+",
                        Woods = sheet.Cells[i, 5].Text == "+",
                        Mountains = sheet.Cells[i, 6].Text == "+",
                        Swamp = sheet.Cells[i, 7].Text == "+",
                        Camp = sheet.Cells[i, 8].Text == "+"
                    };
                    encountersList.Add(enc);
                }
            }
            return encountersList;
        }

        public List<Creature> GetCreaturesList(string excelFileName)
        {
            var creaturesList = new List<Creature>();
            using (var excel = new ExcelPackage(excelFileName))
            {
                var sheet = excel.Workbook.Worksheets["Creatures"];
                for (int i = 2; i < sheet.Dimension.Rows; i++)
                {
                    if (string.IsNullOrEmpty(sheet.Cells[i, 1].Text)) break;
                    var creat = new Creature()
                    {
                        Name = sheet.Cells[i, 1].Text,
                        Source = sheet.Cells[i, 2].Text,
                        UsedInEndlessLegendCampaign = sheet.Cells[i, 3].Text == "+",
                        RuinsUnderground = sheet.Cells[i, 4].Text == "+",
                        PlainsHills = sheet.Cells[i, 5].Text == "+",
                        Desert = sheet.Cells[i, 6].Text == "+",
                        Woods = sheet.Cells[i, 7].Text == "+",
                        Mountains = sheet.Cells[i, 8].Text == "+",
                        Swamp = sheet.Cells[i, 9].Text == "+",
                        Dimensions = sheet.Cells[i, 10].Text == "+",
                        Water = sheet.Cells[i, 11].Text == "+"
                    };
                    creaturesList.Add(creat);
                }
            }
            return creaturesList;
        }
    }
}
