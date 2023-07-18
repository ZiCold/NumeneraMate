using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.NumeneraObjects.Devices
{
    public class Cypher : NumeneraDevice
    {
        public string Wearable { get; set; }
        public string Usable { get; set; }
        public string Internal { get; set; }
    }

}
