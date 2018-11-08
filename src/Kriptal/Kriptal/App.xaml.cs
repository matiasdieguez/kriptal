using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Kriptal.Helpers;
using Kriptal.Resources;
using Kriptal.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Kriptal
{
    public partial class App : Application
    {
        public static byte[] Password { get; set; }

        public static string UriData { get; set; }

        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            var home = string.Empty;
            var contacts = string.Empty;
            var about = string.Empty;
            switch (Device.RuntimePlatform)
            {
                case Device.Windows:
                case Device.WinPhone:
                    home = AppResources.Title;
                    contacts = AppResources.Contacts;
                    about = AppResources.About;
                    break;
            }

            if (!string.IsNullOrEmpty(UriData) && Password != null)
            {
                if (UriData.Contains(UriMessage.KriptalContactUri))
                    Current.MainPage = new NewUserPage();
                if (UriData.Contains(UriMessage.KriptalMessageUri))
                    Current.MainPage = new ReadPage();
            }
            else
            {
                if (Password == null)
                    Current.MainPage = new LoginPage();
                else
                    Current.MainPage = new TabbedPage
                    {
                        Children =
                        {
                            new NavigationPage(new HomePage())
                            {
                                Icon = "home.png",
                                Title = home
                            },
                            new NavigationPage(new UsersPage())
                            {
                                Icon = "contacts.png",
                                Title = contacts
                            },
                            new NavigationPage(new AboutPage())
                            {
                                Icon = "about.png",
                                Title = about
                            },
                        }
                    };
            }
        }
    }
}