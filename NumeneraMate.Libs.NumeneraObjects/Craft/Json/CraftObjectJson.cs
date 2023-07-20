using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Libs.NumeneraObjects.Craft
{
    public class CraftObjectJson
    {
        public CraftObjectJson() { }
        public string Kind { get; set; }
        public string Name { get; set; }
        public string Specifications { get; set; }
        public string Modifications { get; set; }
        public string Depletion { get; set; }
        public int MinCraftingLevel { get; set; }
        public int Parts { get; set; }
        public IotumJson Iotum { get; set; }
    }
}
