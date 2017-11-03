using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Newtonsoft.Json;

using Kriptal.Data;
using Kriptal.Crypto;
using Kriptal.Models;
using Kriptal.Helpers;
using Kriptal.Resources;

namespace Kriptal.ViewModels
{
    public class WriteToUserViewModel : BaseViewModel
    {
        public User User { get; set; }

        private string text = string.Empty;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public Command SendCommand => new Command(async () => await Send());

        public WriteToUserViewModel(User user = null)
        {
            Title = AppResources.Contacts + " to " + user.Name;
            User = user;
        }

        async Task Send()
        {
            IsBusy = true;

            var localDataManager = new LocalDataManager(App.Password);

            var kriptalMsg = new KriptalMessage();

            var sha = new ShaHash();
            var aes = new AesCrypto();
            var rsa = new RsaCrypto();

            var randomGenerator = new RandomGeneration();
            var aesKey = sha.DeriveShaKey(randomGenerator.GetRandomPassword(64), 512).Digest;
            var aesResult = aes.Encrypt(Text, aesKey);

            kriptalMsg.Data = aesResult.EncryptedText;

            kriptalMsg.FromId = rsa.EncryptWithPublic(localDataManager.GetMyId(), User.PublicKey);
            kriptalMsg.Signature = rsa.EncryptWithPrivate(kriptalMsg.FromId, localDataManager.GetPrivateKey());
            kriptalMsg.AesKey = rsa.EncryptWithPublic(aesKey, User.PublicKey);
            kriptalMsg.AesIv = rsa.EncryptWithPublic(Convert.ToBase64String(aesResult.Iv), User.PublicKey);

            var text = UriMessage.KriptalMessageUri + Uri.EscapeDataString(JsonConvert.SerializeObject(kriptalMsg));
            await CrossShare.Current.Share(new ShareMessage { Title = AppResources.FromTitle, Text = text });

            IsBusy = false;
        }
    }
}