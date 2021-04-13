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
    public class XMLArtefactRepo : IUnchangeableRepo<Artefact>
    {
        // If DB is loaded in memory on initializing
        // Factory pattern as in: https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor/34311951#34311951
        public static async Task<XMLArtefactRepo> Create(string xmlFileName)
        {
            var deviceRepo = new XMLArtefactRepo(xmlFileName);
            await Task.Factory.StartNew(() => deviceRepo.Initialize());
            return deviceRepo;
        }

        string _xmlFileName;
        List<Artefact> _artefacts;
        private XMLArtefactRepo(string xmlFileName)
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

            _artefacts = numDevices.Artefacts;
        }

        public List<Artefact> GetAllItems()
        {
            return _artefacts;
        }

        public Task<List<Artefact>> GetAllItemsAsync()
        {
            return Task.FromResult(_artefacts);
        }

        public Artefact GetItem(string name)
        {
            return _artefacts.FirstOrDefault(x => x.Name == name);
        }

        public Task<Artefact> GetItemAsync(string name)
        {
            return Task.FromResult(_artefacts.FirstOrDefault(x => x.Name == name));
        }
    }
}
