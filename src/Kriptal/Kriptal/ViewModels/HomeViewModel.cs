using System.IO;

using Xamarin.Forms;
using QRCoder;
using Newtonsoft.Json;

using Kriptal.Data;
using Kriptal.Models;
using Kriptal.Resources;

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

        public HomeViewModel()
        {
            Title = AppResources.Title;
            GenerateQr();
        }

        public void GenerateQr()
        {
            var localDataManager = new LocalDataManager(App.Password);
            var qrJson = JsonConvert.SerializeObject(new UserItem { Name = localDataManager.GetName(), PublicKey = localDataManager.GetPublicKey() });
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
    }
}
