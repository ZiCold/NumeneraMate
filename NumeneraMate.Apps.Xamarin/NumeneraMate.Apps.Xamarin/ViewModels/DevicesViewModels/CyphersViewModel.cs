using NumeneraMate.Apps.Xamarin.Repos;
using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class CyphersViewModel : BaseViewModel
    {
        public CyphersViewModel()
        {
            Title = "Cyphers";

            GenerateRandomDevice = new Command(async () => await OnGenerateDeviceAsync());

            _xmlFileName = "NumeneraMate.Apps.Xamarin.DevicesFiles.Cyphers_AllSources.xml";
            IsGenerateButtonEnabled = true;
        }
        string _xmlFileName;

        IUnchangeableRepo<Cypher> repo;

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

        List<Cypher> Cyphers { get; set; }

        // Maybe feature IRandom for using Random.org
        Random rand = new Random(Guid.NewGuid().GetHashCode());

        async Task OnGenerateDeviceAsync()
        {
            IsGenerateButtonEnabled = false;
            if (repo is null)
                repo = await XMLCypherRepo.Create(_xmlFileName);
            Cyphers = await repo.GetAllItemsAsync();
            GenerateDevice();
            IsGenerateButtonEnabled = true;
        }

        public void GenerateDevice()
        {
            var randomIndex = rand.Next(Cyphers.Count);
            var diceRandom = rand.Next(1, 6);

            var generatedCypher = Cyphers[randomIndex];
            generatedCypher.Level += $" [D6 = {diceRandom}]";

            Description = generatedCypher.ToString();
        }
    }
}
