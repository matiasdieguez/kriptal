using Kriptal.Helpers;

namespace Kriptal.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string _title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}

