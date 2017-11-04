using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Kriptal.Droid
{
    [Activity(Label = "ContactActivity")]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    DataScheme = "http",
    DataHost = "kriptal.org",
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    public class ContactActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ContactLayout);

            var id = "No Data Found";

            if (Intent.HasExtra("al_applink_data"))
            {

                var appLinkData = Intent.GetStringExtra("data");

            }

            FindViewById<TextView>(Resource.Id.textView1).Text = id;
        }
    }
}