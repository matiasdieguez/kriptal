using Xamarin.Forms.Platform.Android;
using Android.Content;
using Xamarin.Forms;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using Kriptal.Droid;
using Kriptal;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]

namespace Kriptal.Droid
{

    class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
            Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}