using System.Collections.Generic;
using System.Xml.Serialization;

namespace NumeneraMate.Libs.Devices
{

    [XmlRoot("RollTable")]
    public class RollTable
    {
        [XmlElement("Row")]
        public List<RollTableRow> RollTableRows { get; set; }

        public override string ToString()
        {
            var result = "";
            foreach (var r in RollTableRows)
                result += $"\n{r.Roll}: {r.Result}";
            return result;
        }
    }

    public class RollTableRow
    {
        public string Roll { get; set; }
        public string Result { get; set; }
    }
}
