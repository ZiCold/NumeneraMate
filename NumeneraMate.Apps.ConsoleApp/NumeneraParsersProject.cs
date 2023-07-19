using NumeneraMate.Apps.ConsoleApp.PdfTextParsers;
using NumeneraMate.Libs.Craft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp
{
    static class NumeneraParsersProject
    {
        // TODO: Create helper class for parse method
        // TODO: Create console application for file parsing
        // TODO: Add categories into Cyphers & Artefacts XMLs
        // TODO: add minimum crafting level
        public static void MainMethod()
        {
            //CreateXMLfromCSV();
            //ArtefactsCreateXML();
            //CyphersCreateXML();
            //OdditiesCreateXMLTest();
        }

        static void FunWithCraftObjects()
        {
            string dirPath = @"E:\Documents\Tabletop RPGs\Numenera\APPs\";
            string filename = dirPath + "TEST_Installations.txt";
            string testfilename = dirPath + "TEST_Installations.txt";
            string keywordsFile = dirPath + "KEYWORDS_Craft_Objects.txt";

            // first - create Info
            var info = new PDFTextFileInfo
            {
                ItemsFileName = testfilename,
                TableKeyword = ""
            };
            PDFTextParser.LoadKeywordsFromFile(info, keywordsFile);

            var lines = File.ReadAllLines(testfilename);
            // second - get string objects
            var parsedObjects = PDFTextParser.SplitItemsToObjects(info, lines);

            File.Delete(dirPath + "TEST_OUTPUT_InBetween.txt");
            foreach (var obj in parsedObjects)
            {
                File.AppendAllLines(dirPath + "TEST_OUTPUT_InBetween.txt", obj);
            }

            // create Dictionary
            var dic = PDFTextParser.CreateDictionariesFromObjects(parsedObjects, info.KeywordsList);

            // and make XML from it
            var xmlInfo = new PDFTextXmlInfo()
            {
                XmlFileName = dirPath + "TEST_OUTPUT_Installations.xml",
                ObjectsName = "CraftObjects",
                ObjectName = "CraftObject",
                Source = "Destiny"
            };

            PDFTextXmlCreator.CreateXML(xmlInfo, dic, info.TableKeyword);
        }

        static void AppendToFile(string path, List<string> data)
        {

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                foreach (var str in data)
                {
                    sw.WriteLine(str);
                }
            }
        }

        static void AppendToFile(string path, string[] data)
        {
            //using (var sw = File.AppendText(path))
            File.AppendAllLines(path, data);
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                foreach (var str in data)
                {
                    sw.WriteLine(str);
                }
            }
        }

        static void CSVFileMaking(List<CraftObject> craftingList)
        {
            string csvResult = "";
            string delim = ";";
            string newLine = "\n";
            string ret = Environment.NewLine;

            csvResult += craftingList[0].GetCsv(delim, true) + newLine;
            foreach (var obj in craftingList)
            {
                csvResult += obj.GetCsv(delim);
                csvResult += newLine;
            }
            File.WriteAllText("ObjectsList.csv", csvResult);
        }
    }
}
