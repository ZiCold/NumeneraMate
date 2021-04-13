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
    public class XMLCypherRepo : BaseXMLRepo<Cypher>, IUnchangeableRepo<Cypher>
    {
        // If DB is loaded in memory on initializing
        // Factory pattern as in: https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor/34311951#34311951
        public static async Task<XMLCypherRepo> Create(string xmlFileName)
        {
            var deviceRepo = new XMLCypherRepo(xmlFileName);
            await Task.Factory.StartNew(() => deviceRepo.Initialize());
            return deviceRepo;
        }

        private XMLCypherRepo(string xmlFileName)
        {
            _xmlFileName = xmlFileName;
        }

        private void Initialize()
        {
            var numDevices = InitializeNumeneraDevices();

            _devices = numDevices.Cyphers;
        }

        public Cypher GetItem(string name)
        {
            return _devices.FirstOrDefault(x => x.Name == name);
        }

        public Task<Cypher> GetItemAsync(string name)
        {
            return Task.FromResult(_devices.FirstOrDefault(x => x.Name == name));
        }
    }
}
