using NumeneraMate.Apps.Xamarin.Repos;
using NumeneraMate.Libs.Devices;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class ArtefactsViewModel : BaseDeviceViewModel<Artefact>
    {
        public ArtefactsViewModel()
        {
            Title = "Artefacts";
            rand = new Random(Guid.NewGuid().GetHashCode());

            GenerateRandomDevice = new Command(async () => await OnGenerateDeviceAsync());

            _xmlFileName = "NumeneraMate.Apps.Xamarin.DevicesFiles.Artefacts_AllSources.xml";
            IsGenerateButtonEnabled = true;
        }

        string _xmlFileName;

        Artefact _artefact;
        public Artefact Artefact
        {
            get => _artefact;
            set => SetProperty(ref _artefact, value);
        }

        public Command GenerateRandomDevice { get; }

        async Task OnGenerateDeviceAsync()
        {
            IsGenerateButtonEnabled = false;
            if (repo is null)
                repo = await XMLArtefactRepo.Create(_xmlFileName);
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

            Artefact = curDevice;
        }
    }
}
