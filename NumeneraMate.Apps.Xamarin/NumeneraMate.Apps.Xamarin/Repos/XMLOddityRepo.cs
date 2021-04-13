using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NumeneraMate.Apps.Xamarin.Repos
{
    public class XMLOddityRepo : IUnchangeableRepo<Oddity>
    {
        // If DB is loaded in memory on initializing
        // Factory pattern as in: https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor/34311951#34311951
        public static async Task<XMLOddityRepo> Create(string xmlFileName)
        {
            var deviceRepo = new XMLOddityRepo(xmlFileName);
            await Task.Factory.StartNew(() => deviceRepo.Initialize());
            return deviceRepo;
        }

        string _xmlFileName;
        List<Oddity> _devices;
        private XMLOddityRepo(string xmlFileName)
        {
            _xmlFileName = xmlFileName;
        }

        private void Initialize()
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

            _devices = numDevices.Oddities;
        }

        public List<Oddity> GetAllItems()
        {
            return _devices;
        }

        public Task<List<Oddity>> GetAllItemsAsync()
        {
            return Task.FromResult(_devices);
        }

        public Oddity GetItem(string name)
        {
            return _devices.FirstOrDefault(x => x.Description == name);
        }

        public Task<Oddity> GetItemAsync(string name)
        {
            return Task.FromResult(_devices.FirstOrDefault(x => x.Description == name));
        }
    }
}
