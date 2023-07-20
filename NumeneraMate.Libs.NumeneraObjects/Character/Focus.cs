using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NumeneraMate.Libs.NumeneraObjects.Character
{
    public class Focus
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Flavor { get; set; }
        public List<Ability> Connection { get; set; } = new List<Ability>();
        public List<Ability> Additional { get; set; } = new List<Ability>();

        [JsonPropertyName("Minor-Major Effects")]
        public List<Ability> MinorMajorEffects { get; set; } = new List<Ability>();

        [JsonPropertyName("Tier 1")]
        public List<Ability> Tier1 { get; set; } = new List<Ability>();

        [JsonPropertyName("Tier 2")]
        public List<Ability> Tier2 { get; set; } = new List<Ability>();

        [JsonPropertyName("Tier 3 (choose one)")]
        public List<Ability> Tier3ChooseOne { get; set; } = new List<Ability>();

        [JsonPropertyName("Tier 4")]
        public List<Ability> Tier4 { get; set; } = new List<Ability>();

        [JsonPropertyName("Tier 5")]
        public List<Ability> Tier5 { get; set; } = new List<Ability>();

        [JsonPropertyName("Tier 6")]
        public List<Ability> Tier6ChooseOne { get; set; } = new List<Ability>();
    }
}
