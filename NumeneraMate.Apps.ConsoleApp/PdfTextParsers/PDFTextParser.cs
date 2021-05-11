using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NumeneraMate.Apps.ConsoleApp.PdfTextParsers
{
    /// <summary>
    /// Parses text with Numenera items (cyphers, artefacts) from PDF
    /// Use:
    /// 1. FixBrokenWords
    /// 2. SplitLinesByKeywords
    /// 3. SplitItemsToObjects
    /// </summary>
    public class PDFTextParser
	{
		/// <summary>
		/// Example of using
		/// </summary>
		/// <param name="info"></param>
		/// <param name="outputFilename"></param>
		public static void CreateClearedFileFromMessedUp(PDFTextFileInfo info, string outputFilename)
		{
			var linesFromFile = File.ReadAllLines(info.ItemsFileName);
			var itemsLines = new List<string>(linesFromFile);

			itemsLines = PDFTextParser.FixBrokenWords(itemsLines);
			File.WriteAllLines(info.ItemsFileName + "_FixedWords.txt", itemsLines);

			itemsLines = PDFTextParser.SplitLinesByKeywords(itemsLines, info.KeywordsList);
			File.WriteAllLines(info.ItemsFileName + "_SplitByKeyWords.txt", itemsLines);

			var objects = PDFTextParser.SplitItemsToObjects(info, itemsLines.ToArray());

			var smthToWrite = new List<string>();
			objects.ForEach(x => x.ForEach(s => smthToWrite.Add(s)));
			File.WriteAllLines(info.ItemsFileName + "_Cleared.txt", smthToWrite);
		}


		#region ClearFileStrings

		/// <summary>
		/// Clears strings obtained from file of broken words
		/// </summary>
		/// <param name="lines">Lines from file</param>
		/// <returns></returns>
		public static List<string> FixBrokenWords(List<string> lines)
		{
			Dictionary<string, string> WordFixes = new Dictionary<string, string>()
			{
				{ "Eﬀect", "Effect" },
				{ "eﬀect", "effect" },
				{ "suﬀer", "suffer"},
				{ "aﬀect", "affect"},
				{ "diﬀerent", "different"},
				{ "inﬂict", "inflict"},
				{ "ﬂesh", "flesh"},
				{ "ﬂex", "flex"},
				{ "ﬂoors", "floors"},
				{ "afxe", "affixe"},
				{ "afixes", "affixes"},
				{ "solidifis", "solidifies"},


				{ "Effct", "Effect"},
				{ "effct", "effect"},
				{ "Effrt", "Effort"},
				{ "inflct", "inflict"},

				{ "fve", "five"},
				{ "feld", "field"},
				{ "specifc", "specific"},
				{ "specife", "specifie"},
				{ "difculty", "difficulty"},
				{ "difcult", "difficult"},
				{ "fngertip", "fingertip"},
				{ "fne", "fine"},
				{ "fre", "fire"},
				{ "frst", "first"},
				{ "fnal", "final"},
				{ "fxes", "fixes"},
				{ "flls", "fills"},
				{ "flsh", "flesh"},
				{ "fies", "fires"},


				{ "artifcially", "artificially"},
				{ "indefnitely", "indefinitely"},
				{ "microflaments", "microfilaments"},
				{ "identifes", "identifies"},
			};

			var result = new List<string>();
			foreach (var line in lines)
			{
				var clearLine = line;
				// fix broken words
				foreach (var wordFix in WordFixes)
				{
					if (line.Contains(wordFix.Key))
						clearLine = clearLine.Replace(wordFix.Key, wordFix.Value);
				}
				result.Add(clearLine);
			}

			return result;
		}

		/// <summary>
		/// If in the line a few keywords - split it
		/// </summary>
		/// <param name="itemsLines">Lines from file</param>
		/// <param name="keywordsList">Keywords</param>
		/// <returns></returns>
		internal static List<string> SplitLinesByKeywords(List<string> itemsLines, List<string> keywordsList)
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
					keyWordPos = FindFirstKeyWordInLine(curLine, keywordsList);
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

		/// <summary>
		/// Helper method for SplitLinesByKeyword
		/// </summary>
		/// <param name="line"></param>
		/// <param name="keywords"></param>
		/// <returns></returns>
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

		#endregion

		#region Create List of Object from lines

		/// <summary>
		/// Splits lines to different Numenera objects, each line - one keyword
		/// </summary>
		/// <param name="info"></param>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static List<List<string>> SplitItemsToObjects(PDFTextFileInfo info, string[] lines, string nameKeyWord = "")
		{
			var result = new List<List<string>>();
			var curObj = new List<string>();

			for (int i = 0; i < lines.Length; i++)
			{
				// start with the title
				curObj.Add(nameKeyWord + lines[i]);
				i = BuildCurrentObject(lines, i, curObj, info);

				// add current object to the result
				result.Add(new List<string>(curObj));
				curObj.Clear();
			}
			return result;
		}


		/// <summary>
		/// Helper: Creates features for the object except it's title
		/// </summary>
		/// <param name="lines"></param>
		/// <param name="index"></param>
		/// <param name="curObj"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		private static int BuildCurrentObject(string[] lines, int index, List<string> curObj, PDFTextFileInfo info)
		{
			// read current object body starting from the next string
			for (int j = index + 1; j < lines.Length; j++)
			{
				if (string.IsNullOrEmpty(lines[j]))
				{
					if (j + 1 == lines.Length) return j;
					continue;
				}
				// if next line is not the end
				if (j + 1 < lines.Length)
				{
					// If this line is title - start new object
					if (lines[j + 1].Contains(info.KeywordsList.First()))
						return j - 1;
				}
				else
				{
					// if it was the last line, check for keyword for the last time
					// or add to exixsting object and finish
					if (info.KeywordsList.Any(s => lines[j].Contains(s)))
						curObj.Add(lines[j]);
					else
						curObj[curObj.Count - 1] += lines[j] + " ";
					return j;
				}

				// check line for Keyword and add new line
				if (info.KeywordsList.Any(s => lines[j].Contains(s)))
					curObj.Add("");

				// if it's table, than build it OR add normal keyword/line
				if (info.TableKeyword != null && lines[j].Contains(info.TableKeyword))
				{
					curObj[curObj.Count - 1] += lines[j] + " ";
					j = BuildTable(lines, j, curObj, info.KeywordsList);
				}
				else
				{
					curObj[curObj.Count - 1] += lines[j] + " ";
				}

			}
			return -1;
		}

		/// <summary>
		/// Helper: Builds table with rows delimited by symbol #
		/// </summary>
		/// <param name="lines"></param>
		/// <param name="index"></param>
		/// <param name="curObj"></param>
		/// <param name="keywordsList"></param>
		/// <returns></returns>
		private static int BuildTable(string[] lines, int index, List<string> curObj, List<string> keywordsList)
		{
			//curObj[curObj.Count - 1] += " ";
			for (int k = index + 1; k < lines.Length; k++)
			{
				if (string.IsNullOrEmpty(lines[k])) continue;
				// if next line is not the end
				if (k + 1 < lines.Length)
				{
					// If this line is title - start new object
					if (keywordsList.Any(s => lines[k + 1].Contains(s)))
					{
						// add current row to the table
						curObj[curObj.Count - 1] += "#" + lines[k];
						return k;
					}
					if (keywordsList.Any(s => lines[k].Contains(s)))
						return k - 1;
				}
				else
				{
					// if it was the last line than end
					curObj[curObj.Count - 1] += "#" + lines[k];
					return k - 1;
				}

				curObj[curObj.Count - 1] += "#" + lines[k];
			}
			return -1;
		}

		#endregion

		/// <summary>
		/// Loads Keywords from file and check if there is any TableKeyword marked by symbol #
		/// </summary>
		/// <param name="info"></param>
		public static void LoadKeywordsFromFile(PDFTextFileInfo info, string keywordsFilename)
		{
			if (string.IsNullOrEmpty(keywordsFilename))
				throw new ArgumentException("Argument is null", nameof(keywordsFilename));

			string[] keywords;
			try
			{
				keywords = File.ReadAllLines(keywordsFilename);
			}
			catch (Exception ex)
			{
				ex.Data["UserMessage"] += "There is an error with loading keywords from file;";
				throw;
			}

			info.KeywordsList = new List<string>(keywords);

			if (info.TableKeyword == null)
			{
				// check for # and note table keyword
				for (int i = 0; i < info.KeywordsList.Count; i++)
				{
					if (info.KeywordsList[i].First() == '#')
					{
						info.KeywordsList[i] = info.KeywordsList[i].Substring(1);
						info.TableKeyword = info.KeywordsList[i];
					}
				}
			}
		}



		/// <summary>
		/// Method to parse oddities lines from file to strings
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static List<string> ParseFileWithRollTableToStrings(string filename)
		{
			var lines = File.ReadAllLines(filename);
			for (int i = 0; i < lines.Length; i++)
			{
				int spaceIndex = lines[i].IndexOf(' ');
				lines[i] = lines[i].Substring(spaceIndex + 1, lines[i].Length - spaceIndex - 1);
			}
			return lines.ToList();
		}



		/// <summary>
		/// Creates dictionary Keywords : StringValues
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static List<Dictionary<string, string>> CreateDictionariesFromObjects(List<List<string>> objects, List<string> keywordsList)
		{
			var result = new List<Dictionary<string, string>>();
			foreach (var obj in objects)
			{
				result.Add(new Dictionary<string, string>());
				result.Last().Add("Name", obj[0]);

				// loop through object's strings
				for (int i = 1; i < obj.Count; i++)
				{
					// looking for keyword
					foreach (var keyword in keywordsList)
					{
						if (obj[i].StartsWith(keyword))
						{
							// get clear feature string
							string featureValue = obj[i].Substring(keyword.Length).Trim();

							// get clear keyword string
							string featureName = keyword.Trim();

							// if keys happens to be two times (inlikely)
							if (result.Last().Keys.Contains(keyword))
								result.Last()[keyword] += " " + featureValue;
							else
								result.Last().Add(featureName, featureValue);

						}
					}
				}
			}
			return result;
		}
	}
}
