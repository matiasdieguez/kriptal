using Xamarin.Forms;
using Kriptal.Views;
using Kriptal.Crypto;

namespace Kriptal.ViewModels
{
    public class LoginViewModel
    {
        public Command EnterCommand { get; set; }

        public LoginViewModel()
        {
            EnterCommand = new Command(() => ExecuteEnterCommand());
        }

        void ExecuteEnterCommand()
        {
            var crypto = new RsaCrypto();
            var keys = crypto.CreateKeyPair();
            var text = "hola como te va";
            var encrypted = crypto.RsaEncryptWithPublic(text, keys.PublicKey);
            var decrypted = crypto.RsaDecryptWithPrivate(encrypted, keys.PrivateKey);


            Application.Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Inbox",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Contacts",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                }
            };
        }
    }
}
