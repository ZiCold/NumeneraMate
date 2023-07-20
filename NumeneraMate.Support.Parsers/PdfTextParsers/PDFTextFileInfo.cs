namespace NumeneraMate.Support.Parsers.PdfTextParsers
{
	/// <summary>
	/// Class is used by PDFTextParser
	/// </summary>
	public class PDFTextFileInfo
	{
		/// <summary>
		/// Name of the file with Numenera items
		/// </summary>
		public string ItemsFileName { get; set; }

		/// <summary>
		/// Keywords (such as Form:, Effect:, Depletion:)
		/// </summary>
		public List<string> KeywordsList { get; set; }

		/// <summary>
		/// Keyword which says that until next keyword table will go
		/// </summary>
		public string TableKeyword { get; set; }
	}
}
