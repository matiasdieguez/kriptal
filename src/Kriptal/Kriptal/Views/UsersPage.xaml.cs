using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Kriptal.Models;
using Kriptal.ViewModels;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPage : ContentPage
    {
        UsersViewModel _viewModel;

        public UsersPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new UsersViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as User;
            if (item == null)
                return;

            await Navigation.PushAsync(new WriteToUserPage(new WriteToUserViewModel(item)));

            // Manually deselect item
            UsersListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewUserPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Users.Count == 0)
                _viewModel.LoadUsersCommand.Execute(null);
        }
    }
}
