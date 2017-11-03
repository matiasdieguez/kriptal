using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;

using Kriptal.Views;
using Kriptal.Crypto;
using Kriptal.Resources;
using Kriptal.Data;

namespace Kriptal.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command EnterCommand => new Command(async () => await Enter());

        public Command ResetCommand => new Command(async () => await Reset());

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

        private string repeatPassword = string.Empty;
        public string RepeatPassword
        {
            get => repeatPassword;
            set => SetProperty(ref repeatPassword, value);
        }

        private bool isNewAccount;
        public bool IsNewAccount
        {
            get => isNewAccount;
            set => SetProperty(ref isNewAccount, value);
        }

        public bool NotForNewAccount
        {
            get => !isNewAccount;
        }

        public string PasswordInfo { get { return string.Format(AppResources.SetPassword, GetSamplePassword()); } }

        public bool IsAccountReset { get; set; }

        public LoginViewModel()
        {
            IsNewAccount = !LocalDataManager.ExistsPassword();
        }

        public string GetSamplePassword()
        {
            return new RandomGeneration().GetRandomPassword();
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
                array.Any(c => char.IsSymbol(c) ||
                               char.IsPunctuation(c) ||
                               char.IsSeparator(c)) &&
                array.Any(char.IsDigit))
                return true;
            else
                return false;
        }

        async Task Reset()
        {
            var result = await Application.Current.MainPage.DisplayAlert(AppResources.Title, AppResources.ResetMessage, AppResources.ResetAccount, AppResources.Cancel);
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Title, AppResources.ResetCompleted, AppResources.OK);
                IsNewAccount = IsAccountReset = true;
            }
        }

        async Task Enter()
        {
            IsBusy = true;

            if (IsNewAccount && Password != RepeatPassword)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert(AppResources.Title, AppResources.PasswordDontMatch, AppResources.OK);
                return;
            }

            if (IsNewAccount && !CheckStrongPassword(Password))
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert(AppResources.Title, AppResources.MustBeStrongPassword, AppResources.OK);
                return;
            }

            try
            {
                var sha = new ShaHash();

                LocalDataManager localDataManager;

                if (!LocalDataManager.ExistsPassword() || IsAccountReset)
                {
                    var hash = sha.DeriveShaKey(Password, 64);

                    localDataManager = new LocalDataManager(Convert.FromBase64String(hash.Digest));
                    LocalDataManager.SaveSaltBytes(hash.Salt);
                    App.Password = Convert.FromBase64String(hash.Digest);

                    if (IsAccountReset)
                        localDataManager.DeleteDb();

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

                    var test = localDataManager.GetPrivateKey();
                    if (string.IsNullOrEmpty(test))
                        throw new Exception(AppResources.IncorrectPassword);
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
}
