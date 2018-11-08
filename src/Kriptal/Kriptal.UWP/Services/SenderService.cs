using Kriptal.Resources;
using Kriptal.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(Kriptal.UWP.Services.SenderService))]
namespace Kriptal.UWP.Services
{
    public class SenderService : ISender
    {
        public byte[] GetFileData(string path)
        {
            return null;
        }

        public string GetFilePath(string fileName)
        {
            return string.Empty;
        }

        public void SaveFile(string path, byte[] file)
        {
        }

        public void Send(string text, string to)
        {
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage
            {
                Body = text
            };

            if (to != null)
            {
                var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(to);
                emailMessage.To.Add(emailRecipient);
                emailMessage.Subject = AppResources.FromTitle;
            }

            Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        public void Send(string text, string to, byte[] file, string fileName)
        {
        }
    }
}
