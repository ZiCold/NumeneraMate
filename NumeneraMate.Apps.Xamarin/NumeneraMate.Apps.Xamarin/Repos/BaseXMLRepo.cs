using NumeneraMate.Libs.Devices;
using NumeneraMate.Support.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace NumeneraMate.Apps.Xamarin.Repos
{
    public abstract class BaseXMLRepo<T>
        where T : class, new()
    {
        protected string _xmlFileName;
        protected List<T> _devices;

        protected NumeneraDevices InitializeNumeneraDevices()
        {
            var numDevices = new NumeneraDevices();
            var assembly = IntrospectionExtensions.GetTypeInfo(this.GetType()).Assembly;
            Stream stream = assembly.GetManifestResourceStream(_xmlFileName);
            using (var reader = new System.IO.StreamReader(stream))
            {
                var xmlString = reader.ReadToEnd();
                XmlSerializer ser = new XmlSerializer(typeof(NumeneraDevices));
                using (TextReader xmlStringReader = new StringReader(xmlString))
                {
                    numDevices = (NumeneraDevices)ser.Deserialize(xmlStringReader);
                }
            }
            return numDevices;
        }

        public List<T> GetAllItems()
        {
            return _devices;
        }

        public Task<List<T>> GetAllItemsAsync()
        {
            return Task.FromResult(_devices);
        }
    }
}
