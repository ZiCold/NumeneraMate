using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumeneraMate.Support.Parsers
{
	/// <summary>
	/// Clears strings obtained from file from broken words
	/// </summary>
	public static class TextFromPdfWordsFixer
    {
        public static Dictionary<string, string> WordFixesList { get; set; } =
			new Dictionary<string, string>()
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

		public static List<string> ClearText(string textToFix)
        {
			var textLines = textToFix.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
			return ClearText(textLines);
        }

		public static List<string> ClearText(List<string> textLinesToFix)
		{
			var clearedText = new List<string>();
			foreach (var line in textLinesToFix)
			{
				var clearLine = line;
				
				foreach (var wordFix in WordFixesList)
				{
					if (line.Contains(wordFix.Key))
						clearLine = clearLine.Replace(wordFix.Key, wordFix.Value);
				}
				clearedText.Add(clearLine);
			}

			return clearedText;
		}
	}
}
