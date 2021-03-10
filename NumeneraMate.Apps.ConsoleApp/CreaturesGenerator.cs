using NumeneraMate.Libs.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp
{
    class CreaturesGenerator
    {
        public List<Creature> Creatures { get; set; }
        public List<Creature> CreaturesFullList { get; set; }

        public Random Rand { get; set; }
        public List<string> TerrainList { get; set; }
        public CreaturesGenerator()
        {

        }
        public static CreaturesGenerator GetDefaultGenerator()
        {
            var gen = new CreaturesGenerator();
            gen.Rand = new Random(Guid.NewGuid().GetHashCode());
            gen.CreaturesFullList = CreaturesParser.GetCreaturesListFromExcel(@"C:\Users\ZiCold\OneDrive\TRPGs - Numenera\NumeneraAppFiles\", @"Creatures and Events Table.xlsx");
            gen.Creatures = gen.CreaturesFullList.Where(x => x.UsedInEndlessLegendCampaign).ToList();
            gen.TerrainList = new List<string>()
                {
                    "Ruins/Underground",
                    "Plains/Hills",
                    "Desert",
                    "Woods",
                    "Mountains",
                    "Swamp",
                    "Dimensions",
                    "Water"
                };
            return gen;
        }

        public string GetCreature(string terrainType)
        {
            var terrainCreatures = GetCreaturesListByTerrain(terrainType);
            var randomIndex = Rand.Next(Creatures.Count);
            var output = $"Terrain type: {terrainType}" + Environment.NewLine +
                $"Number of creatures: {terrainCreatures.Count}" + Environment.NewLine +
                terrainCreatures[randomIndex].ToString() + Environment.NewLine;
            return output;
        }

        private List<Creature> GetCreaturesListByTerrain(string terrainType)
        {
            var terrainCreatures = new List<Creature>();
            foreach (var creat in Creatures)
            {
                var prop = creat.GetType().GetProperties().Where(y => y.Name == terrainType).FirstOrDefault();
                if ((bool)prop.GetValue(creat)) terrainCreatures.Add(creat);
            }
            return terrainCreatures;
        }
    }
}
