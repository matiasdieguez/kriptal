using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using System;

namespace Kriptal.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
        DataScheme = "http",
        DataHost = "kriptal.org",
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            //request permissions
            while (!VerifyPermissions(this)){}

            var uriData = Intent?.Data?.ToString();

            App.UriData = uriData;
            var app = new App();

            LoadApplication(app);
        }

        public static bool VerifyPermissions(Activity context)
        {
            var hasPermissions = false;
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, Manifest.Permission.Camera) != (int)Permission.Granted)
                ActivityCompat.RequestPermissions(context, new string[] { Manifest.Permission.Camera }, 0);
            hasPermissions = Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, Manifest.Permission.Camera) == (int)Permission.Granted;

            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                ActivityCompat.RequestPermissions(context, new string[] { Manifest.Permission.WriteExternalStorage }, 2);
            hasPermissions = hasPermissions && Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted;

            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
                ActivityCompat.RequestPermissions(context, new string[] { Manifest.Permission.ReadExternalStorage }, 2);
            hasPermissions = hasPermissions && Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted;

            return hasPermissions;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}