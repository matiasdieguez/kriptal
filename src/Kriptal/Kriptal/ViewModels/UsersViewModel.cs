using System;
using System.Diagnostics;
using System.Linq;
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
        public ObservableRangeCollection<UserItem> Users { get; set; }
        public Command LoadUsersCommand => new Command(async () => await LoadUsers());

        public UsersViewModel()
        {
            Title = AppResources.Contacts;
            Users = new ObservableRangeCollection<UserItem>();

            MessagingCenter.Subscribe<NewUserPage, User>(this, "AddItem", (obj, item) =>
            {
                var _item = item as User;
                Users.Add(new UserItem { Id = _item.Id, Name = _item.Name, PublicKey = _item.PublicKey });
            });
        }

        async Task LoadUsers()
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
                    var items = users.Select(i => new UserItem { Id = i.Id, Name = i.Name, PublicKey = i.PublicKey });
                    Users.ReplaceRange(items);
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