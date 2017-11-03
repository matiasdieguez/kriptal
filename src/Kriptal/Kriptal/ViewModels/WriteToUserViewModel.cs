using Kriptal.Models;
using Kriptal.Resources;

namespace Kriptal.ViewModels
{
    public class WriteToUserViewModel : BaseViewModel
    {
        public User User { get; set; }
        public WriteToUserViewModel(User user = null)
        {
            Title = AppResources.Contacts + " to " + user.Name;
            User = user;
        }

    }
}