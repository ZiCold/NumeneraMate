using NumeneraMate.Libs.NumeneraObjects.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Support.Parsers.FromExcel
{
    internal class CharactersAbilitiesParser
    {
        public static List<Ability> GetAbilitiesList(string abilitiesText)
        {
            if (string.IsNullOrEmpty(abilitiesText)) return new List<Ability>();
            var result = new List<Ability>();

            var split = abilitiesText.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

            foreach (var line in split)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var colonIndex = line.IndexOf(':');
                if (colonIndex == -1)
                {
                    result.Add(new Ability() { Description = line });
                }
                else
                {
                    var ability = new Ability()
                    {
                        Description = line.Substring(colonIndex + 1, line.Length - colonIndex - 1).Trim()
                    };

                    var abilityName = line.Substring(0, colonIndex);

                    var bracket = abilityName.IndexOf('(');
                    if (bracket > 0)
                    {
                        abilityName = line.Substring(0, bracket - 1);
                        var cost = line[bracket + 1].ToString();
                        if (char.IsDigit(line[bracket + 2]) || line[bracket + 2] == '+') cost += line[bracket + 2];
                        if (char.IsDigit(line[bracket + 3]) || line[bracket + 3] == '+') cost += line[bracket + 3];
                        if (char.IsDigit(line[bracket + 4]) || line[bracket + 4] == '+') cost += line[bracket + 4];

                        var forPoolFind = line.Substring(bracket, colonIndex);
                        var forPoolSplitted = forPoolFind.Split(' ');
                        var pool = forPoolSplitted[1];
                        ability.Cost = cost;
                        ability.Pool = pool;
                    }
                    ability.Name = abilityName;
                    result.Add(ability);
                }

            }
            return result;
        }
    }
}
