using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Kriptal.Droid;
using Kriptal;

[assembly: ExportRenderer(typeof(MyButton), typeof(MyButtonRenderer))]

namespace Kriptal.Droid
{
    class MyButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.RoundedButton);
            }
        }
    }
}