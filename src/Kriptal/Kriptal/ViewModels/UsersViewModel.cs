using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Kriptal.Helpers;
using Kriptal.Models;
using Kriptal.Views;

using Xamarin.Forms;
using Kriptal.Data;
using Kriptal.Resources;

namespace Kriptal.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        public ObservableRangeCollection<User> Users { get; set; }
        public Command LoadUsersCommand { get; set; }

        public UsersViewModel()
        {
            Title = AppResources.Contacts;
            Users = new ObservableRangeCollection<User>();
            LoadUsersCommand = new Command(async () => await ExecuteLoadUsersCommand());

            MessagingCenter.Subscribe<NewUserPage, User>(this, "AddItem", (obj, item) =>
            {
                var _item = item as User;
                Users.Add(_item);
            });
        }

        async Task ExecuteLoadUsersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Users.Clear();

                await Task.Run(() =>
                {
                    var users = new LocalDataManager(App.Password).List<User>();
                    Users.ReplaceRange(users);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load users.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}