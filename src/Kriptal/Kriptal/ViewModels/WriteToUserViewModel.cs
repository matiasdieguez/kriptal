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
using Kriptal.Services;
using Plugin.FilePicker;
using System.IO;
using System.Text;

namespace Kriptal.ViewModels
{
    public class WriteToUserViewModel : BaseViewModel
    {
        public User User { get; set; }

        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _fileName = string.Empty;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private string _filePath = string.Empty;
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        private byte[] _fileBytes = null;
        public byte[] FileBytes
        {
            get => _fileBytes;
            set => SetProperty(ref _fileBytes, value);
        }

        public Command SendCommand => new Command(async () => await Send());
        public Command AttachFileCommand => new Command(async () => await AttachFile());

        public WriteToUserViewModel(User user = null)
        {
            Title = AppResources.Contacts + AppResources.To + user.Name;
            User = user;
        }

        async Task AttachFile()
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                FileName = file.FileName;
                FilePath = file.FilePath;
                FileBytes = file.DataArray;
            }
            else
            {
                FileName = string.Empty;
                FilePath = string.Empty;
                FileBytes = null;
            }
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
            var aesKey = sha.DeriveShaKey(randomGenerator.GetRandomPassword(64), 16).Digest;

            var aesResult = aes.Encrypt(Text, aesKey);
            kriptalMsg.TextData = aesResult.EncryptedText;

            if (FileName != string.Empty)
            {
                var fileAesKey = sha.DeriveShaKey(new RandomGeneration().GetRandomPassword(64), 16).Digest;
                var aesFileResult = aes.Encrypt(Encoding.UTF8.GetString(FileBytes, 0, FileBytes.Length), fileAesKey);

                kriptalMsg.FileData = aesFileResult.EncryptedText;
                kriptalMsg.FileName = rsa.EncryptWithPublic(FileName, User.PublicKey); ;
                kriptalMsg.FileAesKey = rsa.EncryptWithPublic(fileAesKey, User.PublicKey);
                kriptalMsg.FileAesIv = rsa.EncryptWithPublic(Convert.ToBase64String(aesFileResult.Iv), User.PublicKey);
            }

            kriptalMsg.FromId = rsa.EncryptWithPublic(localDataManager.GetMyId(), User.PublicKey);
            //kriptalMsg.Signature = rsa.EncryptWithPrivate(kriptalMsg.FromId, localDataManager.GetPrivateKey());
            kriptalMsg.TextAesKey = rsa.EncryptWithPublic(aesKey, User.PublicKey);
            kriptalMsg.TextAesIv = rsa.EncryptWithPublic(Convert.ToBase64String(aesResult.Iv), User.PublicKey);

            var text = UriMessage.KriptalMessageUri + Uri.EscapeDataString(JsonConvert.SerializeObject(kriptalMsg));
            //await CrossShare.Current.Share(new ShareMessage { Title = AppResources.FromTitle, Url = text });
            var sender = DependencyService.Get<ISender>();

            //if (FileName != string.Empty)
            //{
            //    sender.Send(text, User.Email, FileBytes, FileName);
            //}
            //else

            sender.Send(text, User.Email);

            IsBusy = false;
        }
    }
}