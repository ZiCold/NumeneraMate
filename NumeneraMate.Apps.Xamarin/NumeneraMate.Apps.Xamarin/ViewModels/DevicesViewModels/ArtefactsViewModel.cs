using NumeneraMate.Apps.Xamarin.Repos;
using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class ArtefactsViewModel : BaseViewModel
    {
        public ArtefactsViewModel()
        {
            Title = "Artefacts";

            GenerateRandomDevice = new Command(async () => await OnGenerateDeviceAsync());

            _xmlFileName = "NumeneraMate.Apps.Xamarin.DevicesFiles.Artefacts_AllSources.xml";
            IsGenerateButtonEnabled = true;
        }

        string _xmlFileName;

        IUnchangeableRepo<Artefact> repo;

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

        List<Artefact> Artefacts { get; set; }

        // Maybe feature IRandom for using Random.org
        Random rand = new Random(Guid.NewGuid().GetHashCode());

        async Task OnGenerateDeviceAsync()
        {
            IsGenerateButtonEnabled = false;
            if (repo is null)
                repo = await XMLArtefactRepo.Create(_xmlFileName);
            Artefacts = await repo.GetAllItemsAsync();
            GenerateDevice();
            IsGenerateButtonEnabled = true;
        }

        public void GenerateDevice()
        {
            var randomIndex = rand.Next(Artefacts.Count);
            var diceRandom = rand.Next(1, 6);

            var generatedCypher = Artefacts[randomIndex];
            generatedCypher.Level += $" [D6 = {diceRandom}]";

            Description = generatedCypher.ToString();
        }
    }
}
