using NumeneraMate.Apps.Xamarin.Repos;
using NumeneraMate.Libs.Devices;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class CyphersViewModel : BaseDeviceViewModel<Cypher>
    {
        public CyphersViewModel()
        {
            Title = "Cyphers";
            rand = new Random(Guid.NewGuid().GetHashCode());

            GenerateRandomDevice = new Command(async () => await OnGenerateDeviceAsync());

            _xmlFileName = "NumeneraMate.Apps.Xamarin.DevicesFiles.Cyphers_AllSources.xml";
            IsGenerateButtonEnabled = true;
        }

        string _xmlFileName;

        Cypher _cypher;
        public Cypher Cypher
        {
            get => _cypher;
            set => SetProperty(ref _cypher, value);
        }

        public Command GenerateRandomDevice { get; }

        async Task OnGenerateDeviceAsync()
        {
            IsGenerateButtonEnabled = false;
            if (repo is null)
                repo = await XMLCypherRepo.Create(_xmlFileName);
            Devices = await repo.GetAllItemsAsync();
            GenerateDevice();
            IsGenerateButtonEnabled = true;
        }

        public void GenerateDevice()
        {
            var randomIndex = rand.Next(Devices.Count);

            var curDevice = Devices[randomIndex];
            
            var randomLevel = curDevice.LevelBaseDice == 0 ? 0 : rand.Next(1, curDevice.LevelBaseDice);
            randomLevel += curDevice.LevelIncrease;
            curDevice.CurrentLevel = randomLevel;

            Cypher = curDevice;
        }
    }
}
