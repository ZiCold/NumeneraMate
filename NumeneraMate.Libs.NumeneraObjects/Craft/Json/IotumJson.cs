using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NumeneraMate.Libs.NumeneraObjects.Craft
{
    public class IotumJson
    {
        public int Io { get; set; }

        [JsonPropertyName("Responsive Synth")]
        public int ResponsiveSynth { get; set; }

        [JsonPropertyName("Apt Clay")]
        public int AptClay { get; set; }

        [JsonPropertyName("Pliable Metal")]
        public int PliableMetal { get; set; }

        [JsonPropertyName("Mimetic Gel")]
        public int MimeticGel { get; set; }

        [JsonPropertyName("Amber Crystal")]
        public int AmberCrystal { get; set; }
        public int Psiranium { get; set; }
        public int Oraculum { get; set; }

        [JsonPropertyName("Tamed Iron")]
        public int TamedIron { get; set; }

        [JsonPropertyName("Cosmic Foam")]
        public int CosmicFoam { get; set; }
    }
}
