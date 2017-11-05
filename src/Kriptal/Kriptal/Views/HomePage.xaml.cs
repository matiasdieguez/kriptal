using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Kriptal.ViewModels;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HomeViewModel();
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ScanContactPage());
        }
    }
}