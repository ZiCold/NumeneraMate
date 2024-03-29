﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Libs.NumeneraObjects.Events
{
    public class Creature
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public bool UsedInEndlessLegendCampaign { get; set; }
        public bool RuinsUnderground { get; set; }
        public bool PlainsHills { get; set; }
        public bool Desert { get; set; }
        public bool Woods { get; set; }
        public bool Mountains { get; set; }
        public bool Swamp { get; set; }
        public bool Dimensions { get; set; }
        public bool Water { get; set; }

        public override string ToString()
        {
            var result = $"Name: {this.Name}{Environment.NewLine}" +
                $"Source: {this.Source}";

            return result;
        }
    }
}
