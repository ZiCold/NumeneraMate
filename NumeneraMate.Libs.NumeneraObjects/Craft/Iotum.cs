using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace NumeneraMate.Libs.NumeneraObjects.Craft
{
	/// <summary>
	/// Class defines basic method for Iotum
	/// </summary>
	public class Iotum
	{
		// set properties in default values
		public Iotum() { }

		/// <summary>
		/// creates Iotum object from the following string (example)
		/// Io(1d6 units); responsive synth(45 units); synthsteel(50 units)
		/// </summary>
		/// <param name="strToParse">Input string</param>
		/// <param name="delimeter">Delimiter</param>
		public Iotum(string strToParse, string delimeter)
		{
			// get separate iotum
			if (strToParse == null) return;
			string[] lines = strToParse.Split(new[] { delimeter }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				// get name for property
				var braketPos = line.IndexOf('(');
				var propName = GetPropertyName(line.Substring(0, braketPos));

				// calculate units
				var unitsToParse = line.Substring(braketPos);
				var valueToParse = unitsToParse.Substring(1, unitsToParse.IndexOf('u') - 1);

				if (valueToParse.Contains("d"))
					valueToParse = valueToParse.Substring(0, valueToParse.IndexOf('d'));
				uint value = (uint)int.Parse(valueToParse);
				try
				{
					this.GetType().GetProperty(propName).SetValue(this, value);
				}
				catch (NullReferenceException)
				{
					throw new NullReferenceException("There is no such type in this class.");
				}
			}
		}

		/// <summary>
		/// Converts word or words from input string to property's name
		/// </summary>
		/// <param name="strToConvert">One iotum name</param>
		/// <returns></returns>
		string GetPropertyName(string strToConvert)
		{
			var inProcess = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strToConvert.ToLower());
			var charsToRemove = new string[] { " ", "-" };
			foreach (var c in charsToRemove)
				inProcess = inProcess.Replace(c, string.Empty);
			return inProcess;
		}

		/// <summary>
		/// Creates CSV for Iotum only
		/// </summary>
		/// <param name="delimeter"></param>
		/// <param name="titles"></param>
		/// <returns></returns>
		public string GetCSV(string delimeter, bool titles = false)
		{
			var iotumList = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			string result = "";
			foreach (var io in iotumList)
			{
				var temp = io.GetValue(this).ToString();
				if (temp != "0")
					result += temp;
				result += delimeter;
			}
			return result;
		}

		/// <summary>
		/// Returns all properties names for current class
		/// </summary>
		/// <param name="delimeter"></param>
		/// <returns></returns>
		public string GetCSVTitles(string delimeter)
		{
			var iotumList = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			string result = "";
			foreach (var io in iotumList)
			{
				result += io.Name;
				result += delimeter;
			}
			return result;
		}

		/// <summary>
		/// Compares iotum with any number of properties
		/// </summary>
		/// <param name="obj">Another Iotum</param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Iotum)) return false;
			var toCompareWith = obj as Iotum;

			var iotumList = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var io in iotumList)
			{
				var propName = io.Name;
				var comparedValue = (uint)toCompareWith.GetType().GetProperty(propName).GetValue(toCompareWith);
				var thisValue = (uint)io.GetValue(this);
				if (comparedValue != thisValue)
					return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
