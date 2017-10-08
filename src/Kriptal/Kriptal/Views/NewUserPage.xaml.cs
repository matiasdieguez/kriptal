using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Kriptal.Models;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUserPage : ContentPage
    {
        public User User { get; set; }

        public NewUserPage()
        {
            InitializeComponent();

            User = new User
            {
                Name = "",
                PublicKey = "",
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", User);
            await Navigation.PopToRootAsync();
        }
    }
}