using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.DataAccess
{
	[Serializable, XmlRoot("Cyphers")]
	public class CyphersList
	{
		[XmlElement("Cypher")]
		public List<Cypher> Cyphers { get; set; }

		[XmlAttribute("source")]
		public string Source { get; set; }

		[XmlAttribute("quantity")]
		public string Quantity { get; set; }
	}

	//[Serializable, XmlRoot("Cypher")]
	public class Cypher
	{
		public string Name { get; set; }
		public string Level { get; set; }
		public int MinimumCraftingLevel { get; set; }

		// sum property
		//public string Appearance { get; set; }
		public string Wearable { get; set; }
		public string Usable { get; set; }
		public string Internal { get; set; }

		public string Effect { get; set; }

		// need to be structure
		//[XmlArrayItem("Row")]
		//public RollTableRow[] RollTable { get; set; }
		public RollTable RollTable { get; set; }

		public override string ToString()
		{
			string result = "";
			foreach (var p in this.GetType().GetProperties())
			{
				var name = p.Name;
				var value = p.GetValue(this, null);
				if (value != null) result += $"{name}: {value.ToString()}\n";
			}
			return result;
		}
	}

	[XmlRoot("RollTable")]
	public class RollTable
	{
		[XmlElement("Row")]
		public List<RollTableRow> RollTableRows { get; set; }

		public override string ToString()
		{
			var result = "";
			foreach (var r in RollTableRows)
				result += $"\n{r.Roll}: {r.Result}";
			return result;
		}
	}

	public class RollTableRow
	{
		public string Roll { get; set; }
		public string Result { get; set; }
	}
}
