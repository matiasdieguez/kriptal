using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Kriptal.ViewModels;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WriteToUserPage : ContentPage
    {
        WriteToUserViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
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
