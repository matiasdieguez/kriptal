using System;
using System.Linq;
using Xamarin.Forms;

using Kriptal.Views;
using Kriptal.Crypto;
using Kriptal.Resources;
using Kriptal.Data;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Kriptal.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command EnterCommand => new Command(async () => await ExecuteEnterCommand());

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
        }

        /// <summary>
        /// CheckStrongPassword
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        public static bool CheckStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(password))
                return false;

            var array = password.ToCharArray();

            if (array.Length >= 8 &&
                array.Any(char.IsLetter) &&
                array.Any(char.IsUpper) &&
                array.Any(char.IsLower) &&
                array.Any(char.IsSymbol) &&
                array.Any(char.IsDigit))
                return true;
            else
                return false;
        }

        async Task ExecuteEnterCommand()
        {
            IsBusy = true;

            if (!CheckStrongPassword(Password))
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert(AppResources.Title, AppResources.MustBeStrongPassword, AppResources.OK);
                return;
            }

            try
            {
                var sha = new ShaHash();

                LocalDataManager localDataManager;

                if (!LocalDataManager.ExistsPassword())
                {
                    var hash = sha.DeriveShaKey(Password, 64);

                    localDataManager = new LocalDataManager(Convert.FromBase64String(hash.Digest));
                    LocalDataManager.SaveSaltBytes(hash.Salt);
                    App.Password = Convert.FromBase64String(hash.Digest);

                    localDataManager.CreateDb();

                    var rsa = new RsaCrypto();
                    var keys = await rsa.CreateKeyPair();
                    localDataManager.SavePrivateKey(keys.PrivateKey);
                    localDataManager.SavePublicKey(keys.PublicKey);
                }
                else
                {
                    var key = sha.DeriveShaKey(Password, 64, LocalDataManager.GetSaltBytes());
                    localDataManager = new LocalDataManager(Convert.FromBase64String(key.Digest));
                    App.Password = Convert.FromBase64String(key.Digest);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert(AppResources.Title, AppResources.IncorrectPassword, AppResources.OK);
                return;
            }
            IsBusy = false;

            Application.Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new HomePage())
                    {
                        Icon = "home.png"
                    },
                    new NavigationPage(new UsersPage())
                    {
                        Icon = "contacts.png"
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Icon = "about.png"
                    },
                }
            };
        }
    }
}
