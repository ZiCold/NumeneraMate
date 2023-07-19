using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Libs.NumeneraObjects.Events
{
    public class Encounter
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PlainsHills { get; set; }
        public string Desert { get; set; }
        public string Woods { get; set; }
        public string Mountains { get; set; }
        public string Swamp { get; set; }
        public string Camp { get; set; }
    }
}
