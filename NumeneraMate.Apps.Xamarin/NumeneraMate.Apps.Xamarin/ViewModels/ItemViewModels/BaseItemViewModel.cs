using NumeneraMate.Apps.Xamarin.Models;
using NumeneraMate.Apps.Xamarin.Services;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.ViewModels
{
    public class BaseItemViewModel : BaseViewModel
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
    }
}
