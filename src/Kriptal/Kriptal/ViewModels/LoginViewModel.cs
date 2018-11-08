using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Kriptal.Crypto;
using Kriptal.Resources;
using Kriptal.Data;

namespace Kriptal.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command EnterCommand => new Command(async () => await Enter());

        public Command ResetCommand => new Command(async () => await Reset());

        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _repeatPassword = string.Empty;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set => SetProperty(ref _repeatPassword, value);
        }

        private bool _isNewAccount;
        public bool IsNewAccount
        {
            get => _isNewAccount;
            set => SetProperty(ref _isNewAccount, value);
        }

        public bool NotForNewAccount
        {
            get => !_isNewAccount;
        }

        public string PasswordInfo { get { return string.Format(AppResources.SetPassword, GetSamplePassword()); } }

        public bool IsAccountReset { get; set; }

        public LoginViewModel()
        {
            IsNewAccount = !LocalDataManager.ExistsPassword();
        }

        public string GetSamplePassword()
        {
            return new RandomGeneration().GetRandomPassword(10);
        }

        public static bool CheckStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(password))
                return false;

            var array = password.ToCharArray();

            if (array.Length >= 10 &&
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

            if (IsNewAccount && (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name)))
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert(AppResources.Title, AppResources.NameRequired, AppResources.OK);
                return;
            }

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

                if (IsNewAccount || !LocalDataManager.ExistsPassword() || IsAccountReset)
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
                    localDataManager.SaveMyId(Guid.NewGuid().ToString());
                    localDataManager.SaveName(Name);
                    localDataManager.SaveEmail(Email);
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

            App.SetMainPage();
            IsBusy = false;
        }
    }
}
