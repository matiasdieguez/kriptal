using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V7.App;

namespace Kriptal.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/kriptal", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}