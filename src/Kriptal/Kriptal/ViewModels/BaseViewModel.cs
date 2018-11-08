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

        string _title = string.Empty;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}

