using NumeneraMate.Apps.Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace NumeneraMate.Apps.Xamarin.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}