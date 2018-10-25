using Android.Content;
using Kriptal.Resources;
using Kriptal.Services;

using Xamarin.Forms;

[assembly: Dependency(typeof(Kriptal.Droid.Services.SenderService))]
namespace Kriptal.Droid.Services
{
    public class SenderService : ISender
    {
        public void Send(string text, string to)
        {
            var intent = new Intent();

            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraSubject, AppResources.FromTitle);
            intent.PutExtra(Intent.ExtraTitle, AppResources.Title);
            intent.PutExtra(Intent.ExtraText, text);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            Android.App.Application.Context.StartActivity(intent);
        }

        public void Send(string text, string to, string fileUri)
        {
            var intent = new Intent();

            intent.SetType("mail/*");

            var uri = Android.Net.Uri.Parse(fileUri);

            intent.PutExtra(Intent.ExtraSubject, AppResources.FromTitle);
            intent.PutExtra(Intent.ExtraTitle, AppResources.Title);
            intent.PutExtra(Intent.ExtraText, text);
            intent.PutExtra(Intent.ActionSendto, to);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.PutExtra(Intent.ExtraStream, uri);

            Android.App.Application.Context.StartActivity(intent);
        }
    }
}