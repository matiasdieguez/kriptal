using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Share;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Kriptal.Models;
using Kriptal.Data;
using Kriptal.Crypto;
using Kriptal.Services;
using Kriptal.Resources;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadPage : ContentPage
    {
        public Command OkCommand => new Command(() => App.SetMainPage());
        public Command OpenAttachmentCommand => new Command(async () => await OpenAttachment());
        public Command OpenBlockchainReciptCommand => new Command(async () => await OpenBlockchainRecipt());
        public string MessageText { get; set; } = string.Empty;
        public string BlockchainReciptUrl { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
        public bool HasAttachment { get; set; } = false;

        public string FileData
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

            try
            {
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

                if (!string.IsNullOrEmpty(message.BlockchainStampUrl))
                {
                    var blockchainUrl = rsa.DecryptWithPrivate(message.BlockchainStampUrl, privateKey);
                    BlockchainReciptUrl = blockchainUrl;
                }

                if (message.FileName != string.Empty)
                {
                    FileName = rsa.DecryptWithPrivate(message.FileName, privateKey);
                    var fileAesKey = rsa.DecryptWithPrivate(message.FileAesKey, privateKey);
                    var fileAesIv = rsa.DecryptWithPrivate(message.FileAesIv, privateKey);
                    FileData = aes.Decrypt(message.FileData, fileAesKey, Convert.FromBase64String(fileAesIv));
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
            catch
            {
                DisplayAlert(AppResources.Title, AppResources.CantRead, AppResources.OK);
            }
        }

        async Task OpenAttachment()
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FileData))
            {
                await DisplayAlert(AppResources.Title, AppResources.NoAttachment, AppResources.OK);
                return;
            }

            await Task.Run(() =>
            {
                var sender = DependencyService.Get<ISender>();
                var bytes = System.Text.Encoding.UTF8.GetBytes(FileData);
                sender.SaveFile(FileName, bytes);
            });
        }

        async Task OpenBlockchainRecipt()
        {
            if (string.IsNullOrEmpty(BlockchainReciptUrl))
            {
                await DisplayAlert(AppResources.Title, AppResources.NoBlochckainRecipt, AppResources.OK);
                return;
            }

            using (var client = new System.Net.Http.HttpClient())
            {
                var id = "94e5471b109d384";
                var token = "b2cac4d5-0df4-41e9-fb85-ba001bced70e";

                var authData = $"{id}:{token}";
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeaderValue);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await client.GetAsync(BlockchainReciptUrl);
                var cont = await response.Content.ReadAsByteArrayAsync();
                var jsonResponse = Encoding.UTF8.GetString(cont, 0, cont.Length);

                await DisplayAlert(AppResources.Title, jsonResponse, AppResources.OK);
            }
        }
    }
}