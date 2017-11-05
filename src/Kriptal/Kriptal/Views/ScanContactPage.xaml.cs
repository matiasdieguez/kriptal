using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using Newtonsoft.Json;
using Kriptal.Models;
using Kriptal.Helpers;

namespace Kriptal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanContactPage : ContentPage
    {
        /// <summary>
        /// ZXingScannerView
        /// </summary>
        ZXingScannerView _scannerView;

        /// <summary>
        /// ActivateEnrolledPage
        /// </summary>
        public ScanContactPage()
        {
            InitializeComponent();

            InitializeQr();
        }

        /// <summary>
        /// InitializeQr
        /// </summary>
        private void InitializeQr()
        {
            _scannerView = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingScannerView",
            };

            _scannerView.AutoFocus();

            _scannerView.OnScanResult += (result) =>
               Device.BeginInvokeOnMainThread(async () =>
               {
                   await ProcessQr(result.Text);
               });

            var overlay = new Grid { AutomationId = "zxingDefaultOverlay" };
            overlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//0
            overlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//1
            overlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//2
            overlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//3
            overlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//4
            overlay.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });//0
            overlay.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });//1
            overlay.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });//2
            overlay.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });//3
            overlay.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });//4

            //Imagen solo de forma ilustrativa, responsiva
            Image imageButton = new Image
            {
                Source = "backscanner.png",
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
            };

            //Boton real con funcionalidad
            var back = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
            };
            back.Clicked += OnBackButtonClicked;
            overlay.Children.Add(imageButton, 2, 4);
            overlay.Children.Add(back, 2, 4);

            Image scannerImage = new Image
            {
                Source = "scannerhud.png",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.Fill,
            };
            overlay.Children.Add(scannerImage, 0, 0);
            Grid.SetColumnSpan(scannerImage, 5);
            Grid.SetRowSpan(scannerImage, 4);

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            grid.Children.Add(_scannerView);
            grid.Children.Add(overlay);

            Content = grid;

        }

        /// <summary>
        /// ProcessProduct
        /// </summary>
        /// <param name="result">Result ZXing</param>
        /// <returns>bool</returns>
        private async Task<bool> ProcessQr(string qrData)
        {
            _scannerView.IsAnalyzing = false;
            _scannerView.IsScanning = false;

            try
            {
                var user = JsonConvert.DeserializeObject<UserItem>(qrData);
                App.UriData = UriMessage.KriptalContactUri + Uri.EscapeDataString(qrData);
                App.Current.MainPage = new NewUserPage();
            }
            catch
            {
            }

            return true;
        }

        /// <summary>
        /// OnBackButtonClicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            GoBack();
        }

        /// <summary>
        /// OnAppearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _scannerView.IsScanning = true;
        }

        /// <summary>
        /// OnDisappearing
        /// </summary>
        protected override void OnDisappearing()
        {
            try
            {
                base.OnDisappearing();
                if (_scannerView != null)
                    _scannerView.IsScanning = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// OnBackButtonPressed
        /// </summary>
        /// <returns>bool</returns>
        protected override bool OnBackButtonPressed()
        {
            return GoBack();
        }

        /// <summary>
        /// GoBack
        /// </summary>
        /// <returns>bool</returns>
        private bool GoBack()
        {
            try
            {

                if (_scannerView != null)
                    _scannerView.IsScanning = false;

                Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return true;
        }
    }
}