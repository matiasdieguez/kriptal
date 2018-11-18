using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
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

            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.RoundedCornerEntry);
                Control.SetPadding(20, 10, 10, 4);
            }
        }
    }
}