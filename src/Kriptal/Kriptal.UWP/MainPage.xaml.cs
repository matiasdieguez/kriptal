namespace Kriptal.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new Kriptal.App());
        }
    }
}