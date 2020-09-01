using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Support.Parsers
{
    /// <summary>
    /// Creates xml/csv from from PDF extracted text
    /// </summary>
    public class NumeneraDevicesParser
    {
        public string FileName { get; set; }
        public string Source { get; set; }
        public List<string> KeywordsList { get; set; }
        
        // Input - fileName, source, keywords
        // TODO: add enum for devices and default keywords?
        public NumeneraDevicesParser(string fileName, string sourceBook, List<string> keywordsList)
        {
            FileName = fileName;
            Source = sourceBook;
            KeywordsList = keywordsList;
        }

        public NumeneraDevicesParser(string fileName, string sourceBook, DeviceType numeneraDeviceType)
        {
            FileName = fileName;
            Source = sourceBook;
            switch (numeneraDeviceType)
            {
                case DeviceType.Cypher:
                    KeywordsList = new List<string>() { "Level:", "Internal:", "Wearable:", "Usable:", "Effect:", "#Table:" }; break;
                case DeviceType.Artefact:
                    KeywordsList = new List<string>() { "Level:", "Form:", "Effect:", "#Table:", "Depletion:" }; break;
                case DeviceType.Oddity:
                    KeywordsList = new List<string>(); break;
            }
        }
    }
}
