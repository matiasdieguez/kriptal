using System.IO;

using Xamarin.Forms;
using QRCoder;

using Kriptal.Data;
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
            var myPublicKey = new LocalDataManager(App.Password).GetPublicKey();
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(myPublicKey, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new PngByteQRCode(qrCodeData))
                {
                    var qrCodeImage = qrCode.GetGraphic(20);
                    QrCode = ImageSource.FromStream(() => new MemoryStream(qrCodeImage));
                }
            }
        }
    }
}
