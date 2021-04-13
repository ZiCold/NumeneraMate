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
    public class XMLCypherRepo : IUnchangeableRepo<Cypher>
    {
        // If DB is loaded in memory on initializing
        // Factory pattern as in: https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor/34311951#34311951
        public static async Task<XMLCypherRepo> Create(string xmlFileName)
        {
            var cypherRepo = new XMLCypherRepo(xmlFileName);
            await Task.Factory.StartNew(() => cypherRepo.Initialize());
            return cypherRepo;
        }

        string _xmlFileName;
        List<Cypher> _cyphers;
        private XMLCypherRepo(string xmlFileName)
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

            _cyphers = numDevices.Cyphers;
        }

        public List<Cypher> GetAllItems()
        {
            return _cyphers;
        }

        public Task<List<Cypher>> GetAllItemsAsync()
        {
            return Task.FromResult(_cyphers);
        }

        public Cypher GetItem(string name)
        {
            return _cyphers.FirstOrDefault(x => x.Name == name);
        }

        public Task<Cypher> GetItemAsync(string name)
        {
            return Task.FromResult(_cyphers.FirstOrDefault(x => x.Name == name));
        }
    }
}
