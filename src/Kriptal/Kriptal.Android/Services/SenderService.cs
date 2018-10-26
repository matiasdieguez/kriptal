using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Kriptal.Resources;
using Kriptal.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Kriptal.Droid.Services.SenderService))]
namespace Kriptal.Droid.Services
{
    public class SenderService : ISender
    {
        public byte[] GetFileData(string path)
        {
            var file = File.ReadAllBytes(path);
            return file;
        }

        public string GetFilePath(string fileName)
        {
            string fullPath = Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath,
                                          fileName);
            return fullPath;
        }

        public void Send(string text, string to)
        {
            var intent = new Intent();

            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraSubject, AppResources.FromTitle);
            intent.PutExtra(Intent.ExtraTitle, AppResources.Title);
            intent.PutExtra(Intent.ExtraEmail, to);
            intent.PutExtra(Intent.ExtraText, text);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            Android.App.Application.Context.StartActivity(intent);
        }

        public void Send(string text, string to, byte[] file, string fileName)
        {
            var path = GetFilePath("file.png");
            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllBytes(path, file);
            var uri = Android.Net.Uri.Parse(path);

            var intent = new Intent();

            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraSubject, AppResources.FromTitle);
            intent.PutExtra(Intent.ExtraTitle, AppResources.Title);
            intent.PutExtra(Intent.ExtraText, text);
            intent.PutExtra(Intent.ExtraEmail, to);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.PutExtra(Intent.ExtraStream, uri);

            Android.App.Application.Context.StartActivity(Intent.CreateChooser(intent, "Send email..."));
        }

        public void SaveFile(string path, byte[] file)
        {
            path = Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, "Kriptal-" + path);

            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllBytes(path, file);

            var uri = Android.Net.Uri.Parse(path);
            var intent = new Intent(Intent.ActionView);
            intent.SetData(uri);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(Intent.CreateChooser(intent, "View file"));
        }

    }
}