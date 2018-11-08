using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Kriptal.ViewModels;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WriteToUserPage : ContentPage
    {
        WriteToUserViewModel viewModel;

        public WriteToUserPage()
        {
            InitializeComponent();
        }

        public WriteToUserPage(WriteToUserViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}
