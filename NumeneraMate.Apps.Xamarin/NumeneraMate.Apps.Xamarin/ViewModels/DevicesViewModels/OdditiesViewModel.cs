using NumeneraMate.Apps.Xamarin.Repos;
using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class OdditiesViewModel : BaseDeviceViewModel<Oddity>
    {
        public OdditiesViewModel()
        {
            Title = "Oddities";

            GenerateRandomDevice = new Command(async () => await OnGenerateDeviceAsync());

            _xmlFileName = "NumeneraMate.Apps.Xamarin.DevicesFiles.Oddities_AllSources.xml";
            IsGenerateButtonEnabled = true;
        }
        string _xmlFileName;

        public Command GenerateRandomDevice { get; }

        async Task OnGenerateDeviceAsync()
        {
            IsGenerateButtonEnabled = false;
            if (repo is null)
                repo = await XMLOddityRepo.Create(_xmlFileName);
            Devices = await repo.GetAllItemsAsync();
            GenerateDevice();
            IsGenerateButtonEnabled = true;
        }

        public void GenerateDevice()
        {
            var randomIndex = rand.Next(Devices.Count);

            var generatedOddity = Devices[randomIndex];

            Description = generatedOddity.ToString();
        }
    }
}
