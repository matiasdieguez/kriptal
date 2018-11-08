using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using QRCoder;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Kriptal.Data;
using Kriptal.Models;
using Kriptal.Resources;
using Kriptal.Helpers;
using Kriptal.Views;

namespace Kriptal.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ImageSource _qrCode = null;
        public ImageSource QrCode
        {
            get => _qrCode;
            set => SetProperty(ref _qrCode, value);
        }

        public Command ShareCommand => new Command(async () => await Share());
        public Command ScanCommand => new Command(async () => await Scan());

        public HomeViewModel()
        {
            Title = AppResources.Title;
            GenerateQr();
        }

        public void GenerateQr()
        {
            var localDataManager = new LocalDataManager(App.Password);
            var qrJson = JsonConvert.SerializeObject(new UserItem
            {
                Id = localDataManager.GetMyId(),
                Name = localDataManager.GetName(),
                PublicKey = localDataManager.GetPublicKey(),
                Email = localDataManager.GetEmail()
            });

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrJson, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new PngByteQRCode(qrCodeData))
                {
                    var qrCodeImage = qrCode.GetGraphic(20);
                    QrCode = ImageSource.FromStream(() => new MemoryStream(qrCodeImage));
                }
            }
        }

        async Task Share()
        {
            var localDataManager = new LocalDataManager(App.Password);
            var userItem = new UserItem
            {
                Id = localDataManager.GetMyId(),
                Name = localDataManager.GetName(),
                PublicKey = localDataManager.GetPublicKey(),
                Email = localDataManager.GetEmail()
            };

            var text = UriMessage.KriptalContactUri + Uri.EscapeDataString(JsonConvert.SerializeObject(userItem));
            await CrossShare.Current.Share(new ShareMessage
            {
                Title = AppResources.ContactFromKriptal,
                Url = text
            });
        }

        async Task Scan()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ScanContactPage());
        }
    }
}
