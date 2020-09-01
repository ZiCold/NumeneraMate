using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Support.Parsers
{
    /// <summary>
    /// Creates xml/csv from from PDF extracted text
    /// Some text can highlighted as Table with keyword #Table (rolls for example)
    /// </summary>
    public class DevicesParser
    {
        public string FileName { get; set; }
        public string Source { get; set; }
        public List<string> KeywordsList { get; set; }
        public string NameKeyword { get; set; } = "Name:";

        public DevicesParser(string fileName, string sourceBook, DeviceType numeneraDeviceType)
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
            var textLines = linesArray.Where(x => !string.IsNullOrEmpty(x)).ToList();
            textLines = TextFromPdfWordsFixer.ClearText(textLines);

            // if in a single line more then one keyword
            var keywordsLines = RemoveMoreThanOneKeyWordoccurrences(textLines);

            // Get List of cyphers dictionary List<Dictionary<keyword, string>>
            // cause next we can create XML, CSV, Excel table or return specific objects
            List<Dictionary<string, string>> devicesAsDictionariesList = GetDevicesDictionaries(textLines);



            foreach (var device in devicesAsDictionariesList)
            {
                foreach (var key in device.Keys)
                {
                    Console.WriteLine(key + " " + device[key]);
                }
                Console.WriteLine();
            }

            var cyphersList = new List<Cypher>();
            cyphersList = GetCyphersListFromDictionaries(devicesAsDictionariesList);

            cyphersList.ForEach(x => Console.WriteLine(x));

        }

        private List<Cypher> GetCyphersListFromDictionaries(List<Dictionary<string, string>> devicesAsDictionariesList)
        {
            var cyphersList = new List<Cypher>();
            foreach (var device in devicesAsDictionariesList)
            {
                var cypher = new Cypher() { Source = Source };
                foreach (var key in device.Keys)
                {
                    switch (key)
                    {
                        case "Name:":
                            cypher.Name = device[key]; break;
                        case "Level:":
                            cypher.Level = device[key]; break;
                        case "Internal:":
                            cypher.Internal = device[key]; break;
                        case "Wearable:":
                            cypher.Wearable = device[key]; break;
                        case "Usable:":
                            cypher.Usable = device[key]; break;
                        case "Effect:":
                            cypher.Effect = device[key]; break;
                        case "#Table:":
                            cypher.TableAsString = device[key];
                            cypher.RollTable = new RollTable() { RollTableRows = GetRollListFromTableString(device[key]) };
                            break;
                    }
                }
                cyphersList.Add(cypher);
            }
            return cyphersList;
        }

        private List<RollTableRow> GetRollListFromTableString(string tableString)
        {
            var rollsList = new List<RollTableRow>();
            var rolls = tableString.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var roll in rolls)
            {
                var numberToRoll = "";
                int i = 0;
                for (; i < roll.Length; i++)
                {
                    var symbol = roll[i];
                    if (char.IsDigit(symbol) || char.IsWhiteSpace(symbol) || symbol == '-' || symbol == '–')
                        numberToRoll += symbol;
                    else
                        break;
                }
                var description = roll.Substring(i);
                rollsList.Add(new RollTableRow() { Roll = numberToRoll.Trim(), Result = description.Trim() });
            }
            return rollsList;
        }

        /// <summary>
        /// Create list of Numenera Devices as List of Dictionaries
        /// </summary>
        /// <param name="textLines"></param>
        /// <returns></returns>
        private List<Dictionary<string, string>> GetDevicesDictionaries(List<string> textLines)
        {
            var resultList = new List<Dictionary<string, string>>();
            var linesArray = textLines.ToArray();
            for (int i = 0; i < linesArray.Length; i++)
            {
                i = BuildCurrentDevice(linesArray, i, out var currentDevice);

                var nextObj = new Dictionary<string, string>();
                foreach (var key in currentDevice.Keys)
                    nextObj.Add(key, currentDevice[key].Trim());
                resultList.Add(nextObj);
            }
            return resultList;
        }

        /// <summary>
        /// Creates single cypher or artefact
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="currentIndex"></param>
        /// <param name="curObj"></param>
        /// <returns></returns>
        private int BuildCurrentDevice(string[] lines, int currentIndex, out Dictionary<string, string> curObj)
        {
            curObj = new Dictionary<string, string>();
            curObj.Add(NameKeyword, lines[currentIndex]);

            // read current object body starting from the next string
            var currentKeyword = "";
            int j;
            string tableKeyword = KeywordsList.FirstOrDefault(x => x.StartsWith("#"));
            bool isThereTable = string.IsNullOrEmpty(tableKeyword) ? false : true;
            for (j = currentIndex + 1; j < lines.Length; j++)
            {
                if (string.IsNullOrEmpty(lines[j])) continue;
                // either end of the array or start of the new object
                if (j + 1 != lines.Length && lines[j + 1].StartsWith(KeywordsList.First()))
                    return j - 1;

                // check line for Keyword and add new line
                foreach (var keyword in KeywordsList)
                {
                    if (keyword == tableKeyword && lines[j].StartsWith(keyword.Substring(1)))
                    {
                        currentKeyword = keyword;
                        break;
                    }
                    if (lines[j].StartsWith(keyword))
                    {
                        currentKeyword = keyword;
                        break;
                    }
                }

                if (currentKeyword == "")
                {
                    curObj[NameKeyword] += " " + lines[j];
                    continue;
                }

                if (currentKeyword == tableKeyword)
                {
                    curObj.Add(currentKeyword, "");
                    j = BuildTable(lines, j, out var tableLine);
                    curObj[currentKeyword] = tableLine;
                    continue;
                }

                if (curObj.ContainsKey(currentKeyword))
                {
                    curObj[currentKeyword] += " " + lines[j];
                }
                else
                {
                    var startOfDescription = lines[j].Substring(currentKeyword.Length);
                    curObj.Add(currentKeyword, startOfDescription);
                }
            }
            return j;
        }

        /// <summary>
		/// Helper: Builds table with rows delimited by symbol #
		/// </summary>
		/// <param name="lines"></param>
		/// <param name="index"></param>
		/// <param name="curObj"></param>
		/// <param name="keywordsList"></param>
		/// <returns></returns>
		private int BuildTable(string[] lines, int index, out string tableLine)
        {
            tableLine = "";
            // start build table from the next string after Table keyword
            for (int k = index + 1; k < lines.Length; k++)
            {
                if (string.IsNullOrEmpty(lines[k])) continue;
                // if next line is not the end
                if (k + 1 < lines.Length)
                {
                    // if next line is new keyword, then end table and exit
                    if (KeywordsList.Any(s => lines[k + 1].Contains(s)))
                    {
                        // add current row to the table
                        tableLine += "#" + lines[k];
                        return k;
                    }
                    if (KeywordsList.Any(s => lines[k].Contains(s)))
                        return k - 1;
                }
                else
                {
                    // if it was the last line than end
                    tableLine += "#" + lines[k];
                    return k;
                }

                tableLine += "#" + lines[k];
            }
            return -1;
        }

        /// <summary>
        /// Additional feature - split line if it has more than one keyword
        /// </summary>
        /// <param name="itemsLines"></param>
        /// <returns></returns>
        public List<string> RemoveMoreThanOneKeyWordoccurrences(List<string> itemsLines)
        {
            var result = new List<string>();
            foreach (var line in itemsLines)
            {
                var curLine = line.Trim();
                if (line == "") continue;

                var wholeLineLength = curLine.Length;
                var curLineLength = wholeLineLength;
                do
                {
                    int keyWordPos = FindFirstKeyWordInLine(curLine, KeywordsList);
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
