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

            // The right way is to load interface and then, after repo is loaded, show "data is loaded"
            var task = Task.Factory.StartNew(() => XMLCypherRepo.Create("NumeneraMate.Apps.Xamarin.DevicesFiles.Cyphers_AllSources.xml"));
            repo = task.Result.Result;

            //repo = new XMLCypherRepo("NumeneraMate.Apps.Xamarin.DevicesFiles.Cyphers_AllSources.xml");
        }

        IUnchangeableRepo<Cypher> repo;

        string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command GenerateRandomDevice { get; }

        List<Cypher> Cyphers { get; set; }

        // Maybe feature IRandom for using Random.org
        Random rand = new Random(Guid.NewGuid().GetHashCode());

        async Task OnGenerateDeviceAsync()
        {
            Cyphers = await repo.GetAllItemsAsync();
            GenerateDevice();
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
