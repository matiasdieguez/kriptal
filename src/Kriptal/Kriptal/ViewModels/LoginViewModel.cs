using Xamarin.Forms;

using Kriptal.Views;
using Kriptal.Crypto;
using Kriptal.Resources;

namespace Kriptal.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        public Command EnterCommand { get; set; }

        private string text = string.Empty;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public LoginViewModel()
        {
            EnterCommand = new Command( () => ExecuteEnterCommand());
        }

        void ExecuteEnterCommand()
        {
            IsBusy = true;

            var keyString = new ShaHash().DeriveShaKey(Password, 64);

            Application.Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new HomePage())
                    {
                        Title = AppResources.Home,
                        Icon = "slideout.png"
                    },
                    new NavigationPage(new UsersPage())
                    {
                        Title = AppResources.Contacts,
                        Icon = "slideout.png"
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = AppResources.About,
                        Icon = "slideout.png"
                    },
                }
            };
        }
    }
}
