using NumeneraMate.Libs.NumeneraObjects.Devices;

namespace NumeneraMate.Apps.ConsoleAppDotNet.Parsers
{
    /// <summary>
    /// Creates xml from text extracted from PDF
    /// One part of text can be highlighted as Table with keyword #Table (rolls for example)
    /// </summary>
    public class DevicesParser
    {
        public string Source { get; set; }
        public List<string> KeywordsList { get; set; }
        public string NameKeyword { get; set; } = "Name:";
        public DevicesParser() { }

        /// <summary>
        /// Get cyphers from raw text (from pdf)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<Cypher> GetCyphersListFromRawText(string filename, string source)
        {
            Source = source;
            KeywordsList = new List<string>() { "Level:", "Internal:", "Wearable:", "Usable:", "Effect:", "#Table:" };
            var devicesAsDictionaries = ParseFileToDeviceDictionaries(filename);
            return GetCyphersListFromDictionaries(devicesAsDictionaries);
        }

        /// <summary>
        /// Get artefacts from raw text (from pdf)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<Artefact> GetArtefactsListFromRawText(string filename, string source)
        {
            Source = source;
            KeywordsList = new List<string>() { "Level:", "Form:", "Effect:", "#Table:", "Depletion:" };
            var devicesAsDictionaries = ParseFileToDeviceDictionaries(filename);
            return GetArtefactsListFromDictionaries(devicesAsDictionaries);
        }

        /// <summary>
        /// Get oddities from raw text (from pdf)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<Oddity> GetOdditiesListFromRawText(string fileName, string source)
        {
            var lines = File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "") continue;
                var currentLine = lines[i];
                int j = 0;
                if (char.IsDigit(currentLine[0]))
                {
                    for (; j < currentLine.Length; j++)
                    {
                        if (!char.IsDigit(currentLine[j]) && !char.IsWhiteSpace(currentLine[j]) && currentLine[j] != '.') 
                            break;
                    }
                }
                lines [i] = currentLine.Substring(j);
            }

            var oddities = new List<Oddity>();
            foreach (var line in lines)
            {
                oddities.Add(new Oddity() { Description = line, Source = Source });
            }
            return oddities;
        }

        /// <summary>
        /// Shared method for NumeneraDevices
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<Dictionary<string, string>> ParseFileToDeviceDictionaries(string fileName)
        {
            var linesArray = File.ReadAllLines(fileName);
            var textLines = linesArray.Where(x => !string.IsNullOrEmpty(x)).ToList();
            textLines = TextFromPdfWordsFixer.ClearText(textLines);

            var keywordsLines = RemoveMoreThanOneKeyWordOccurrences(textLines);

            return GetDevicesDictionaries(textLines);
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

        private List<Artefact> GetArtefactsListFromDictionaries(List<Dictionary<string, string>> devicesAsDictionariesList)
        {
            var cyphersList = new List<Artefact>();
            foreach (var device in devicesAsDictionariesList)
            {
                var artefact = new Artefact() { Source = Source };
                foreach (var key in device.Keys)
                {
                    switch (key)
                    {
                        case "Name:":
                            artefact.Name = device[key]; break;
                        case "Level:":
                            artefact.Level = device[key]; break;
                        case "Form:":
                            artefact.Form = device[key]; break;
                        case "Effect:":
                            artefact.Effect = device[key]; break;
                        case "Depletion:":
                            artefact.Depletion = device[key]; break;
                        case "#Table:":
                            artefact.TableAsString = device[key];
                            artefact.RollTable = new RollTable() { RollTableRows = GetRollListFromTableString(device[key]) };
                            break;
                    }
                }
                cyphersList.Add(artefact);
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
        /// Dictionaries are chosen cause next we can create XML, CSV, Excel table or return specific objects
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
                    nextObj.Add(key, ReplaceLongHyphen(currentDevice[key].Trim()));
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
            curObj.Add(NameKeyword, UppercaseFirstLetters(lines[currentIndex]));

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
                if (string.IsNullOrEmpty(lines[k]))
                    continue;
                if (KeywordsList.Any(s => lines[k].StartsWith(s)))
                    return k - 1;

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

                    // for the case if this lines contains line for last roll result
                    if (k + 2 < lines.Length && lines[k + 2].StartsWith(KeywordsList.First()))
                    {
                        if (char.IsDigit(lines[k][0]))
                            tableLine += "#" + lines[k];
                        else
                            tableLine += " " + lines[k];
                        return k;
                    }
                }
                else
                {
                    // if it was the last line than end
                    tableLine += "#" + lines[k];
                    return k;
                }
                if (char.IsDigit(lines[k][0]))
                    tableLine += "#" + lines[k];
                else
                    tableLine += " " + lines[k];
            }
            return -1;
        }

        private string ReplaceLongHyphen(string input)
        {
            return input.Replace('–', '-').Replace('—','-');
        }

        /// <summary>
        /// Additional feature - split line if it has more than one keyword
        /// </summary>
        /// <param name="itemsLines"></param>
        /// <returns></returns>
        private List<string> RemoveMoreThanOneKeyWordOccurrences(List<string> itemsLines)
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
