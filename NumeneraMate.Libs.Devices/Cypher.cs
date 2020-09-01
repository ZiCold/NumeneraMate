using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.Devices
{
    [Serializable, XmlRoot("Cypher")]
    public class Cypher
    {
        private int minimumCraftingLevel;

        public string Name { get; set; }
        public string Level { get; set; }
        public int MinimumCraftingLevel
        {
            get
            {
                if (minimumCraftingLevel == 0 && !string.IsNullOrEmpty(Level))
                    if (Level.Trim() == "1d6")
                        minimumCraftingLevel = 1;
                    else
                        minimumCraftingLevel = int.Parse(Level.Replace("1d6", "").Replace("+", "").Trim()) + 1;
                return minimumCraftingLevel;
            }
            set => minimumCraftingLevel = value;
        }

        public string Wearable { get; set; }
        public string Usable { get; set; }
        public string Internal { get; set; }

        public string Effect { get; set; }

        public RollTable RollTable { get; set; }
        [XmlIgnore]
        public string TableAsString { get; set; }

        public string Source { get; set; }

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

}
