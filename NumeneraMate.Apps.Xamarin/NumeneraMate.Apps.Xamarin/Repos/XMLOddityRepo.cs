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
    public class XMLOddityRepo : BaseXMLRepo<Oddity>, IUnchangeableRepo<Oddity>
    {
        // If DB is loaded in memory on initializing
        // Factory pattern as in: https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor/34311951#34311951
        public static async Task<XMLOddityRepo> Create(string xmlFileName)
        {
            var deviceRepo = new XMLOddityRepo(xmlFileName);
            await Task.Factory.StartNew(() => deviceRepo.Initialize());
            return deviceRepo;
        }
        private XMLOddityRepo(string xmlFileName)
        {
            _xmlFileName = xmlFileName;
        }

        private void Initialize()
        {
            var numDevices = InitializeNumeneraDevices();

            _devices = numDevices.Oddities;
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
