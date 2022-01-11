using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp.XmlModels
{
    public class Encounter
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool PlainsHills { get; set; }
        public bool Desert { get; set; }
        public bool Woods { get; set; }
        public bool Mountains { get; set; }
        public bool Swamp { get; set; }
    }
}
