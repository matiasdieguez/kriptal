using Kriptal.Helpers;
using Kriptal.Resources;
using Kriptal.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

#pragma warning disable CS0618 // Type or member is obsolete
        public static void SetMainPage()
        {
            if (!string.IsNullOrEmpty(UriData) && Password != null)
            {
                if (UriData.Contains(UriMessage.KriptalContactUri))
                    Current.MainPage = new NewUserPage();
                if (UriData.Contains(UriMessage.KriptalMessageUri))
                    Current.MainPage = new NewUserPage();//new ReadMessagePage();
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
                                Title = Device.OnPlatform(null, null, AppResources.Title)

                            },
                            new NavigationPage(new UsersPage())
                            {
                                Icon = "contacts.png",
                                Title = Device.OnPlatform(null, null, AppResources.Contacts)
                            },
                            new NavigationPage(new AboutPage())
                            {
                                Icon = "about.png",
                                Title = Device.OnPlatform(null, null, AppResources.About)
                            },
                        }
                    };
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}