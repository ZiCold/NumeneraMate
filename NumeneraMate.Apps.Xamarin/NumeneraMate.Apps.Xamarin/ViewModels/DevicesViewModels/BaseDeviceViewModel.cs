using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels.DevicesViewModels
{
    public abstract class BaseDeviceViewModel<T> : BaseViewModel
    {
        // Maybe feature IRandom for using Random.org
        protected Random rand;

        protected IUnchangeableRepo<T> repo;

        protected List<T> Devices { get; set; }

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
        
    }
}
