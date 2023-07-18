using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NumeneraMate.Libs.NumeneraObjects.Craft
{
	public class CraftObject : NumeneraObject
	{
		/// <summary>
		/// Sets all values by default
		/// </summary>
		/// <param name="fileName"></param>
		public CraftObject(string fileName)
			: base(fileName)
		{
			KeywordsList = new List<string>
			{
				"Minimum Crafting Level:",
				"Kind:",
				"Iotum:",
				"Parts:",
				"Specifications:",
				"Modification:",
				"Depletion:"
			};
		}

		/// <summary>
		/// Creates full instance of NumeneraObject
		/// </summary>
		/// <param name="itemsFileName"></param>
		/// <param name="keyWordsFileName"></param>
		/// <param name="isLastKeyTable"></param>
		/// <param name="checkTitle">set to null if Title is the first string in the Item</param>
		public CraftObject(string itemsFileName, string keyWordsFileName,
			bool isLastKeyTable, Func<string, bool> checkTitle)
			: base(itemsFileName, keyWordsFileName, isLastKeyTable, checkTitle)
		{ }

		public enum KindOfCraft
		{
			Installation,
			Vehicle,
			Automaton
		}

		[Description("Title")]
		public string Name { get; set; }

		[Description("Minimum Crafting Level")]
		public uint MinCraftingLevel { get; set; }

		[Description("Kind")]
		public KindOfCraft Kind { get; set; }

		[Description("Specifications")]
		public string Specifications { get; set; }

		[Description("Modifications")]
		public string Modifications { get; set; }

		[Description("Depletion")]
		public string Depletion { get; set; }

		[Description("Parts")]
		public uint PartsRequired { get; set; }

		[Description("Iotum")]
		public Iotum IotumRequired { get; set; }



		public void MakeObjectFromStrings(string parsedObj, string featureDelimiter)
		{
			string[] lines = parsedObj.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

			Name = lines[0];
			MinCraftingLevel = (uint)int.Parse(GetFeature(lines[1], featureDelimiter));
			Kind = ParseStringToEnum(lines[2], featureDelimiter);
			var IotumRequiredString = GetFeature(lines[3], featureDelimiter);
			IotumRequired = new Iotum(IotumRequiredString, ";");
			PartsRequired = GetNumberFromUnits(GetFeature(lines[4], featureDelimiter), "unit");
			Specifications = GetFeature(lines[5], featureDelimiter);
			if (lines.Length == 7)
			{
				Modifications = "None";
				Depletion = GetFeature(lines[6], featureDelimiter);
			}
			else
			{
				Modifications = GetFeature(lines[6], featureDelimiter);
				Depletion = GetFeature(lines[7], featureDelimiter);
			}
		}

		/*public static List<CraftObject> ParseXML(string xmlFile)
		{
			var result = new List<CraftObject>();

			XDocument xdoc = XDocument.Load(xmlFile);
			foreach (XElement xmlObj in xdoc.Root.Elements("CraftingObject"))
			{
				var craftObj = new CraftObject();
				var propsList = craftObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

				foreach (XElement feature in xmlObj.Elements())
				{
					var name = feature.Name.LocalName;
					var value = feature.Value;

					if (name != "CraftingComponents")
					{
						var prop = craftObj.GetType().GetProperty(name);
						if (prop == null)
							throw new NullReferenceException("XML has wrong tags");
						dynamic changedObj = prop.PropertyType.IsEnum ?
							Enum.Parse(prop.PropertyType, value, true) :
							Convert.ChangeType(value, prop.PropertyType);
						prop.SetValue(craftObj, changedObj);
					}
					else if (feature.HasElements)
					{
						var iotum = new Iotum();
						foreach (XElement component in feature.Elements())
						{
							var compName = component.Name.LocalName;
							var compValue = (uint)int.Parse(component.Value);
							if (compName == "Parts")
								craftObj.PartsRequired = compValue;
							else
								iotum.GetType().
									GetProperty(compName).
									SetValue(iotum, compValue);
						}
						craftObj.IotumRequired = iotum;
					}
				}
				result.Add(craftObj);
			}
			return result;
		}*/

		// Return text of the feature
		string GetFeature(string feature, string delimeter)
		{
			var pos = feature.IndexOf(delimeter) + delimeter.Length;
			return feature.Substring(pos).Trim();
		}

		// Defines enum value
		KindOfCraft ParseStringToEnum(string kindStr, string delimiter)
		{
			var temp = new KindOfCraft();
			try
			{
				temp = (KindOfCraft)Enum.Parse(typeof(KindOfCraft), GetFeature(kindStr, delimiter));
				if (!Enum.IsDefined(typeof(KindOfCraft), Kind) | Kind.ToString().Contains(","))
					throw new ArgumentException($"{kindStr} is not an underlying value of the CraftingKind enumeration.");
			}
			catch (ArgumentException)
			{
				throw new ArgumentException($"{kindStr} is not a member of the CraftingKind enumeration.");
			}
			return temp;
		}

		// For accountable features such as Parts
		uint GetNumberFromUnits(string featureStr, string unitsName)
		{
			// remove thousands delimeters
			featureStr = featureStr.Replace(",", "");
			var pos = featureStr.IndexOf(unitsName);
			for (int i = 0; i < featureStr.Length; i++)
			{
				if (char.IsLetter(featureStr, i))
				{
					pos = i;
					break;
				}
			}

			return (uint)int.Parse(featureStr.Substring(0, pos));
		}

		public string GetCsv(string delimeter, bool titles = false)
		{
			var propsList = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var result = "";
			foreach (var prop in propsList)
			{
				if (!titles)
				{
					if (prop.Name != "IotumRequired")
						result += prop.GetValue(this).ToString().Replace(";", string.Empty);
					else
						result += (prop.GetValue(this) as Iotum).GetCSV(delimeter);
				}
				else
				{
					if (prop.Name != "IotumRequired")
						result += prop.Name;
					else
						result += (prop.GetValue(this) as Iotum).GetCSV(delimeter, titles);
				}
				result += delimeter;
			}
			return result;
		}

		public override string ToString()
		{
			var ret = new[] { Environment.NewLine };
			return base.ToString();
		}

	}
}
