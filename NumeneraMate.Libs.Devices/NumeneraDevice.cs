using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.Devices
{
    public class NumeneraDevice
    {


        public string Name { get; set; }
        public string Level { get; set; }
        /// <summary>
        /// Base dice for Level calculation
        /// </summary>
        [XmlIgnore]
        public int LevelBase
        {
            get
            {
                var clearedLevelString = Level.Substring(Level.ToLower().IndexOf("d") + 1);
                var maybeDiceValue = "";
                for (int i = 0; i < clearedLevelString.Length; i++)
                {
                    if (clearedLevelString[i] == '+') break;
                    if (char.IsDigit(clearedLevelString[i]))
                        maybeDiceValue += clearedLevelString[i];
                }
                var success = int.TryParse(maybeDiceValue, out int diceValue);
                if (success)
                    return diceValue;
                else
                    return 1;
            }
        }

        [XmlIgnore]
        public int LevelTerm
        {
            get
            {
                if (!Level.Contains("+")) return 0;
                var levelTermStr = Level.Substring(Level.ToLower().IndexOf("+") + 1);
                var success = int.TryParse(levelTermStr, out int levelTerm);
                if (success) return levelTerm;
                else return 0;
            }
        }

        public int MinimumCraftingLevel
        {
            get => 1 + LevelTerm;
        }

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
