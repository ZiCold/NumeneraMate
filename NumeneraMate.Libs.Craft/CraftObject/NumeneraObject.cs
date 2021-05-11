using System;
using System.Collections.Generic;
using System.IO;

namespace NumeneraMate.Libs.Craft
{
	/// <summary>
	/// This class includes all numenera items
	/// </summary>
	public class NumeneraObject
	{
		public NumeneraObject(string itemsFileName)
		{
			InputFile = new FileInfo(itemsFileName);
			IsLastKeywordTable = false;
			CheckTitle = IsAllUpperOrSymbol;
		}

		/// <summary>
		/// Creates full instance of NumeneraObject
		/// </summary>
		/// <param name="itemsFileName"></param>
		/// <param name="keyWordsFileName"></param>
		/// <param name="isLastKeyTable"></param>
		/// <param name="checkTitle">set to null if Title is the first string in the Item</param>
		public NumeneraObject(string itemsFileName, string keyWordsFileName,
			bool isLastKeyTable, Func<string, bool> checkTitle)
		{
			InputFile = new FileInfo(itemsFileName);
			KeywordsFile = new FileInfo(keyWordsFileName);
			IsLastKeywordTable = isLastKeyTable;
			CheckTitle = checkTitle;
		}

		public FileInfo InputFile { get; set; }
		public FileInfo KeywordsFile { get; set; }
		public List<string> KeywordsList { get; set; }
		public bool IsLastKeywordTable { get; set; }
		public Func<string, bool> CheckTitle { get; set; }

		/// <summary>
		/// Loads keywords from keywords file if set
		/// </summary>
		public void LoadKeywordsFromFile()
		{
			if (KeywordsFile == null) return;
			KeywordsList = new List<string>(File.ReadAllLines(KeywordsFile.FullName));
		}

		/// <summary>
		/// Checks title for all upper letters
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsAllUpperOrSymbol(string input)
		{
			if (input == null) return false;
			for (int i = 0; i < input.Length; i++)
			{
				if (char.IsLetter(input[i]) && !char.IsUpper(input[i]))
					//if (input[i] != '(' || input[i] != ')')
					if (char.IsSymbol(input[i]))
						return false;
			}
			return true;
		}
	}
}
