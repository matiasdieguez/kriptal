using Kriptal.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Kriptal
{
    public partial class App : Application
    {
        public static byte[] Password { get; set; }

        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            Current.MainPage = new LoginPage();
        }
    }
}