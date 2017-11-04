using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Kriptal.Models;
using Kriptal.Data;
using Kriptal.Resources;
using Newtonsoft.Json;
using Kriptal.Crypto;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadPage : ContentPage
    {
        public Command OkCommand => new Command(() => App.SetMainPage());

        private string messageText = string.Empty;
        public string MessageText
        {
            get => messageText;
            set => messageText = value;
        }

        private string userName = string.Empty;
        public string UserName
        {
            get => userName;
            set => userName = value;
        }



        public ReadPage()
        {
            InitializeComponent();

            var json = Uri.UnescapeDataString(new Uri(App.UriData).Query.Replace("?data=", string.Empty));
            var message = JsonConvert.DeserializeObject<KriptalMessage>(json);

            var sha = new ShaHash();
            var aes = new AesCrypto();
            var rsa = new RsaCrypto();

            var localDataManager = new LocalDataManager(App.Password);
            var privateKey = localDataManager.GetPrivateKey();

            var aesKey = rsa.DecryptWithPrivate(message.AesKey, privateKey);
            var aesIv = rsa.DecryptWithPrivate(message.AesIv, privateKey);

            var text = aes.Decrypt(message.Data, aesKey, Convert.FromBase64String(aesIv));
            MessageText = text;

            var fromId = rsa.DecryptWithPrivate(message.FromId, privateKey);
            var user = localDataManager.Get<User>(u => u.Id == fromId);
            UserName = user.Name;

            App.UriData = string.Empty;

            BindingContext = this;
        }
    }
}