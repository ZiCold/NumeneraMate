using NumeneraMate.Apps.Xamarin.Repos;
using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class OdditiesViewModel : BaseViewModel
    {
        public OdditiesViewModel()
        {
            Title = "Oddities";

            GenerateRandomDevice = new Command(async () => await OnGenerateDeviceAsync());

            _xmlFileName = "NumeneraMate.Apps.Xamarin.DevicesFiles.Oddities_AllSources.xml";
            IsGenerateButtonEnabled = true;
        }
        string _xmlFileName;

        IUnchangeableRepo<Oddity> repo;

        string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        bool isGenerateButtonEnabled;
        public bool IsGenerateButtonEnabled
        {
            get => isGenerateButtonEnabled;
            set => SetProperty(ref isGenerateButtonEnabled, value);
        }

        public Command GenerateRandomDevice { get; }

        List<Oddity> Devices { get; set; }

        // Maybe feature IRandom for using Random.org
        Random rand = new Random(Guid.NewGuid().GetHashCode());

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
            var diceRandom = rand.Next(1, 6);

            var generatedOddity = Devices[randomIndex];
            //generatedCypher.Level += $" [D6 = {diceRandom}]";

            Description = generatedOddity.ToString();
        }
    }
}
