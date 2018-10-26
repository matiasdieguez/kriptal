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
using Kriptal.Services;

using Plugin.FilePicker;
namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadPage : ContentPage
    {
        public Command OkCommand => new Command(() => App.SetMainPage());
        public Command OpenAttachmentCommand => new Command(async () => await OpenAttachment());

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

        private bool _hasAttachment = false;
        public bool HasAttachment
        {
            get => _hasAttachment;
            set => _hasAttachment = value;
        }

        public string FileInBase64
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public ReadPage()
        {
            InitializeComponent();

            var json = Uri.UnescapeDataString(new Uri(App.UriData).Query.Replace("?data=", string.Empty));
            var message = JsonConvert.DeserializeObject<KriptalMessage>(json);

            var aes = new AesCrypto();
            var rsa = new RsaCrypto();

            var localDataManager = new LocalDataManager(App.Password);
            var privateKey = localDataManager.GetPrivateKey();

            var textAesKey = rsa.DecryptWithPrivate(message.TextAesKey, privateKey);
            var textAesIv = rsa.DecryptWithPrivate(message.TextAesIv, privateKey);
            var text = aes.Decrypt(message.TextData, textAesKey, Convert.FromBase64String(textAesIv));
            MessageText = text;

            if (message.FileName != string.Empty)
            {
                FileName = rsa.DecryptWithPrivate(message.FileName, privateKey);
                var fileAesKey = rsa.DecryptWithPrivate(message.FileAesKey, privateKey);
                var fileAesIv = rsa.DecryptWithPrivate(message.FileAesIv, privateKey);
                FileInBase64 = aes.Decrypt(message.FileData, fileAesKey, Convert.FromBase64String(fileAesIv));
                HasAttachment = true;
            }
            else
                HasAttachment = false;

            var fromId = rsa.DecryptWithPrivate(message.FromId, privateKey);
            var user = localDataManager.Get<User>(u => u.Id == fromId);
            UserName = user.Name;

            App.UriData = string.Empty;

            BindingContext = this;
        }

        async Task OpenAttachment()
        {
            var sender = DependencyService.Get<ISender>();
            var bytes = System.Text.Encoding.UTF8.GetBytes(FileInBase64);
            sender.SaveFile(FileName, bytes);
        }
    }
}