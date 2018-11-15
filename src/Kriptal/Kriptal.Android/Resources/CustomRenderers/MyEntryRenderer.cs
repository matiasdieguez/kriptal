using Xamarin.Forms.Platform.Android;
using Android.Content;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]

namespace Kriptal.Droid.Resources.CustomRenderers
{

    class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(global::Android.Graphics.Color.Transparent);
            Control.SetBackgroundDrawable(gd);
            Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
            Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}