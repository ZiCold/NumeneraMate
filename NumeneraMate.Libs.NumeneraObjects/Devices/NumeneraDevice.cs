using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.NumeneraObjects.Devices
{
    public class NumeneraDevice
    {
        public string Name { get; set; }
        public string Level { get; set; }
        /// <summary>
        /// Base dice for Level calculation
        /// </summary>
        [XmlIgnore]
        public int LevelBaseDice
        {
            get
            {
                var maybeDiceValue = "";
                // if there no 'd' than this is the static level, so no random dice
                // for example 8 = d0 + 8
                if (Level.ToLower().IndexOf("d") == -1)
                {
                    return 0;
                }
                else
                {
                    var clearedLevelString = Level.Substring(Level.ToLower().IndexOf("d") + 1);
                    for (int i = 0; i < clearedLevelString.Length; i++)
                    {
                        if (clearedLevelString[i] == '+') break;
                        if (char.IsDigit(clearedLevelString[i]))
                            maybeDiceValue += clearedLevelString[i];
                    }
                }
                var success = int.TryParse(maybeDiceValue, out int diceValue);
                if (success)
                    return diceValue;
                else
                    return 1;
            }
        }
        /// <summary>
        /// Number after "+" in level string
        /// </summary>
        [XmlIgnore]
        public int LevelIncrease
        {
            get
            {
                var levelTermStr = "";
                if (!Level.Contains("+"))
                {
                    if (!Level.Contains("d")) levelTermStr = Level; // static level, like 8
                    else levelTermStr = "0";      // 1d6
                }
                else
                {
                    levelTermStr = Level.Substring(Level.ToLower().IndexOf("+") + 1);
                }
                var success = int.TryParse(levelTermStr, out int levelTerm);
                if (success) return levelTerm;
                else return 0;
            }
        }
        /// <summary>
        /// random d6 + LevelIncrease
        /// </summary>
        [XmlIgnore]
        public int CurrentLevel { get; set; }

        public int MinimumCraftingLevel
        {
            get => LevelBaseDice != 0 ? 1 + LevelIncrease : LevelIncrease;

            // Uncomment this if you want to serialize MinimumCraftingLevel
            // XmlSerializer does not serialize properties with private setters
            // https://stackoverflow.com/questions/13401192/why-are-properties-without-a-setter-not-serialized
            // set { }
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
