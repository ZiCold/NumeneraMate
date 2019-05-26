using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.DataAccess
{
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

		// need to be structure
		public string RollTable { get; set; }

		public string Effect { get; set; }

		public override string ToString()
		{
			string result = "";
			var props = this.GetType().GetProperties();
			foreach (var p in props)
			{
				var name = p.Name;
				var value = p.GetValue(this, null);
				if (value != null) result += $"{p.Name}: {value.ToString()}\n";
			}
			return result;
		}
	}
}
