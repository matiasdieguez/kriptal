using Kriptal.Models;
using Kriptal.Resources;

namespace Kriptal.ViewModels
{
    public class WriteToUserViewModel : BaseViewModel
    {
        public User User { get; set; }

        private string text = string.Empty;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public WriteToUserViewModel(User user = null)
        {
            Title = AppResources.Contacts + " to " + user.Name;
            User = user;
        }

    }
}