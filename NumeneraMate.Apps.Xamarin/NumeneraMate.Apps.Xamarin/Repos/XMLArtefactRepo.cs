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
    public class XMLArtefactRepo : BaseXMLRepo<Artefact>, IUnchangeableRepo<Artefact>
    {
        // If DB is loaded in memory on initializing
        // Factory pattern as in: https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor/34311951#34311951
        public static async Task<XMLArtefactRepo> Create(string xmlFileName)
        {
            var deviceRepo = new XMLArtefactRepo(xmlFileName);
            await Task.Factory.StartNew(() => deviceRepo.Initialize());
            return deviceRepo;
        }

        private XMLArtefactRepo(string xmlFileName)
        {
            _xmlFileName = xmlFileName;
        }

        private void Initialize()
        {
            var numDevices = InitializeNumeneraDevices();

            _devices = numDevices.Artefacts;
        }

        public Artefact GetItem(string name)
        {
            return _devices.FirstOrDefault(x => x.Name == name);
        }

        public Task<Artefact> GetItemAsync(string name)
        {
            return Task.FromResult(_devices.FirstOrDefault(x => x.Name == name));
        }
    }
}
