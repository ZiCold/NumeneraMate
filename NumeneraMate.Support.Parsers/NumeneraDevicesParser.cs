using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Support.Parsers
{
    /// <summary>
    /// Creates xml/csv from from PDF extracted text
    /// Some text can highlighted as Table with keyword #Table (rolls for example)
    /// </summary>
    public class NumeneraDevicesParser
    {
        public string FileName { get; set; }
        public string Source { get; set; }
        public List<string> KeywordsList { get; set; }
        
        // Input - fileName, source, keywords
        // TODO: maybe add enum for devices and default keywords?
        public NumeneraDevicesParser(string fileName, string sourceBook, List<string> keywordsList)
        {
            FileName = fileName;
            Source = sourceBook;
            KeywordsList = keywordsList;
        }

        public NumeneraDevicesParser(string fileName, string sourceBook, DeviceType numeneraDeviceType)
        {
            FileName = fileName;
            Source = sourceBook;
            switch (numeneraDeviceType)
            {
                case DeviceType.Cypher:
                    KeywordsList = new List<string>() { "Level:", "Internal:", "Wearable:", "Usable:", "Effect:", "#Table:" }; break;
                case DeviceType.Artefact:
                    KeywordsList = new List<string>() { "Level:", "Form:", "Effect:", "#Table:", "Depletion:" }; break;
                case DeviceType.Oddity:
                    KeywordsList = new List<string>(); break;
            }
        }

        public void Run()
        {
            var linesArray = File.ReadAllLines(FileName);
            var textLines = new List<string>();
            foreach (var line in linesArray)
                if (!string.IsNullOrEmpty(line)) textLines.Add(line);
            textLines = TextFromPdfWordsFixer.ClearText(textLines);

            // if in a single line more then one keyword - unnessecary step
            //var keywordsLines = SplitLinesByKeywords(textLines);

            // Get List of cyphers dictionary List<Dictionary<keyword, string>>
            // cause next we can create XML, CSV, Excel table or return specific objects
            List<Dictionary<string, string>> devicesAsDictionariesList = GetDevicesDictionaries(textLines);


            // Get list of cyphers as List of objects

            
            Console.WriteLine();
        }

        private List<Dictionary<string, string>> GetDevicesDictionaries(List<string> textLines)
        {
            throw new NotImplementedException();
        }

        public List<string> SplitLinesByKeywords(List<string> itemsLines)
        {
            var result = new List<string>();
            foreach (var line in itemsLines)
            {
                var curLine = line.Trim();
                if (line == "") continue;

                var wholeLineLength = curLine.Length;
                var curLineLength = wholeLineLength;
                var keyWordPos = curLineLength;
                do
                {
                    keyWordPos = FindFirstKeyWordInLine(curLine, KeywordsList);
                    if (keyWordPos < curLineLength)
                    {
                        var firstPart = curLine.Substring(0, keyWordPos);
                        var secondPart = curLine.Substring(keyWordPos);
                        curLine = secondPart;
                        curLineLength = curLine.Length;
                        result.Add(firstPart);
                    }
                    else
                    {
                        result.Add(curLine);
                        break;
                    }
                }
                while (true);

            }
            return result;
        }

        private static int FindFirstKeyWordInLine(string line, List<string> keywords)
        {
            int position = line.Length;
            foreach (var word in keywords)
            {
                var current = line.IndexOf(word);
                if (current > 0)
                    if (current < position) position = current;
            }
            return position;
        }
    }
}
