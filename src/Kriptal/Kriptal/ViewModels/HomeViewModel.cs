using System.IO;

using Xamarin.Forms;
using QRCoder;
using Newtonsoft.Json;

using Kriptal.Data;
using Kriptal.Models;
using Kriptal.Resources;
using System.Threading.Tasks;
using Kriptal.Helpers;
using System;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace Kriptal.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ImageSource qrCode = null;
        public ImageSource QrCode
        {
            get => qrCode;
            set => SetProperty(ref qrCode, value);
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
                PublicKey = localDataManager.GetPublicKey()
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
            var userItem = new UserItem { Id = localDataManager.GetMyId(), Name = localDataManager.GetName(), PublicKey = localDataManager.GetPublicKey() };
            var text = UriMessage.KriptalContactUri + Uri.EscapeDataString(JsonConvert.SerializeObject(userItem));
            await CrossShare.Current.Share(new ShareMessage
            {
                Title = AppResources.ContactFromKriptal,
                Url = text
            });
        }

        async Task Scan()
        {
            await Share();
        }
    }
}
