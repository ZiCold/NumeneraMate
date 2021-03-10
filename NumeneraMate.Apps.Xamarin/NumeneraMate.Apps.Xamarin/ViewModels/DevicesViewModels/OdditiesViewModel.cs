using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    class OdditiesViewModel : BaseViewModel
    {
        public OdditiesViewModel()
        {
            Title = "Cyphers";

            GeneratedDevices = new ObservableCollection<string>();
            GenerateRandomDevice = new Command(OnGenerateDevice);
        }

        public ObservableCollection<string> GeneratedDevices { get; set; }

        string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command GenerateRandomDevice { get; }
        public Command AddDevice { get; }

        public void OnGenerateDevice()
        {
            Description = "There is and random oddity: Number " + new Random().Next(1, 100) +
                Environment.NewLine + "Oddity Description: RandomDescription" + new Random().Next(1, 100) +
                Environment.NewLine + "Random Level: " + new Random().Next(1, 100);
            GeneratedDevices.Add(Description);
        }
    }
}
