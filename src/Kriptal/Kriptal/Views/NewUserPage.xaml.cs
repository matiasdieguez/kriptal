﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Kriptal.Models;
using Kriptal.Data;
using Kriptal.Resources;

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

            User = new User
            {
                Name = "",
                PublicKey = "",
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
        }

        async Task Save()
        {
            try
            {
                new LocalDataManager(App.Password).Save(new User { Id = User.Id, Name = User.Name, PublicKey = User.PublicKey });
                MessagingCenter.Send(this, "AddItem", User);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DisplayAlert(AppResources.Title, AppResources.Error, AppResources.OK);
            }

            await Navigation.PopToRootAsync();
        }
    }
}