using Xamarin.Forms;
using Kriptal.Views;
using Kriptal.Crypto;
using System.Diagnostics;
using System;

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

        public LoginViewModel()
        {
            EnterCommand = new Command(() => ExecuteEnterCommand());
        }

        async void ExecuteEnterCommand()
        {
            IsBusy = true;
            var timer = new Stopwatch();
            timer.Start();

            var crypto = new RsaCrypto();
            var keysTask = crypto.CreateKeyPair();
            await keysTask.ContinueWith(async (k) =>
            {
                var keys = await k;
                Text = $"Key generation time: {timer.Elapsed.TotalSeconds} secs. {Environment.NewLine}" +
                            $"-BEGIN PUBLIC KEY-: {Environment.NewLine} {keys.PublicKey} {Environment.NewLine} -END PUBLIC KEY-";

                var text = "hola como te va soy mati";
                var cryptoTimer = new Stopwatch();
                cryptoTimer.Start();
                var encrypted = crypto.RsaEncryptWithPublic(text, keys.PublicKey);
                var decrypted = crypto.RsaDecryptWithPrivate(encrypted, keys.PrivateKey);
                cryptoTimer.Stop();
                Text += Environment.NewLine + "Encrypted data: " + Environment.NewLine + encrypted;
                Text += Environment.NewLine + "Decrypted data: " + Environment.NewLine + decrypted;
                Text += Environment.NewLine + "Crypto time: " + Environment.NewLine + cryptoTimer.Elapsed.TotalSeconds;

                var aes = new AesCrypto();
                var keyString = "jDxESdRrcYKmSZi7IOW4lw==";

                var encryptedPrivateKey = aes.Encrypt(keys.PrivateKey, keyString);
                var decryptedPrivateKey = aes.Decrypt(encryptedPrivateKey.EncryptedText, keyString, encryptedPrivateKey.Iv);
                Text += Environment.NewLine + "++++Encrypted Key: " + Environment.NewLine + encryptedPrivateKey;
                Text += Environment.NewLine + "++++Decrypted Key: " + Environment.NewLine + decryptedPrivateKey;

                IsBusy = false;
            });

            return;
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
