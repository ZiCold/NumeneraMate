using NumeneraMate.Apps.Xamarin.Repos;
using NumeneraMate.Libs.Devices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class CyphersViewModel : BaseDeviceViewModel<Cypher>
    {
        public CyphersViewModel()
        {
            Title = "Cyphers";

            GenerateRandomDevice = new Command(async () => await OnGenerateDeviceAsync());

            _xmlFileName = "NumeneraMate.Apps.Xamarin.DevicesFiles.Cyphers_AllSources.xml";
            IsGenerateButtonEnabled = true;
        }

        string _xmlFileName;

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
            var diceRandom = rand.Next(1, 6);

            var generatedCypher = Devices[randomIndex];
            generatedCypher.Level += $" [D6 = {diceRandom}]";

            Description = generatedCypher.ToString();
        }
    }
}
