using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {

        public Command OpenWebCommand { get; }

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = this;
            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/matiasdieguez/kriptal")));
        }
    }
}
