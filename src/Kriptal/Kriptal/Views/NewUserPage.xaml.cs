using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Kriptal.Models;
using Kriptal.Data;
using Kriptal.Resources;
using Newtonsoft.Json;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUserPage : ContentPage
    {
        public Command SaveCommand => new Command(async () => await Save());
        public User User { get; set; }

        public NewUserPage()
        {
            InitializeComponent();

            var json = Uri.UnescapeDataString(new Uri(App.UriData).Query.Replace("?data=", string.Empty));
            var user = JsonConvert.DeserializeObject<UserItem>(json);

            User = new User
            {
                Name = user.Name,
                PublicKey = user.PublicKey,
                Id = user.Id,
                Email = user.Email
            };

            App.UriData = string.Empty;

            BindingContext = this;
        }

        async Task Save()
        {
            try
            {
                new LocalDataManager(App.Password).Save(new User
                {
                    Id = User.Id,
                    Name = User.Name,
                    PublicKey = User.PublicKey,
                    Email = User.Email
                });
                MessagingCenter.Send(this, "AddItem", User);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DisplayAlert(AppResources.Title, AppResources.Error, AppResources.OK);
            }

            //await Navigation.PopToRootAsync();
            App.SetMainPage();
        }
    }
}