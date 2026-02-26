using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenRails_HID_Tester.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Panto1Image = panto1Down;
            Panto2Image = panto2Down;
        }

        private bool _headlightsState = false;          // current logical state
        private bool _isHeadlightsAnimating = false;    // animation lock

        private static Bitmap LoadAsset(string name) =>
            new Bitmap(AssetLoader.Open(new Uri($"avares://OpenRails-HID-Tester/Assets/{name}")));

        [ObservableProperty]
        private string deviceStatus = "Unknown";
        [ObservableProperty]
        private IBrush deviceStatusBrush = Brushes.Red;

        [ObservableProperty]
        private string panto1Status = "";
        [ObservableProperty]
        private IBrush panto1StatusBrush = Brushes.Black;

        [ObservableProperty]
        private string panto2Status = "";
        [ObservableProperty]
        private IBrush panto2StatusBrush = Brushes.Black;

        [ObservableProperty]
        private string headlightsStatus = "";
        [ObservableProperty]
        private IBrush headlightsStatusBrush = Brushes.Black;

        [ObservableProperty]
        private string pauseStatus = "";
        [ObservableProperty]
        private IBrush pauseStatusBrush = Brushes.Black;

        [ObservableProperty]
        private string viewStatus = "";

        [ObservableProperty]
        private string stationMonitorStatus = "";
        [ObservableProperty]
        private IBrush stationMonitorStatusBrush = Brushes.Black;

        [ObservableProperty]
        private string trackMonitorStatus = "";
        [ObservableProperty]
        private IBrush trackMonitorStatusBrush = Brushes.Black;

        [ObservableProperty]
        private string directionStatus = "";
        [ObservableProperty]
        private IBrush directionStatusBrush = Brushes.Black;

        [ObservableProperty]
        private string throttleStatus = "";
        [ObservableProperty]
        private IBrush throttleStatusBrush = Brushes.Black;

        [ObservableProperty]
        private string engineBrakeStatus = "";
        [ObservableProperty]
        private IBrush engineBrakeStatusBrush = Brushes.Black;
        
        [ObservableProperty]
        private string trainBrakeStatus = "";
        [ObservableProperty]
        private IBrush trainBrakeStatusBrush = Brushes.Black;

        // Pantograph pictures
        private readonly Bitmap panto1Up = LoadAsset("Panto1Up.png");
        private readonly Bitmap panto1Down = LoadAsset("Panto1Down.png");
        private readonly Bitmap panto2Up = LoadAsset("Panto2Up.png");
        private readonly Bitmap panto2Down = LoadAsset("Panto2Down.png");

        [ObservableProperty]
        private Bitmap panto1Image;

        [ObservableProperty]
        private Bitmap panto2Image;

        [ObservableProperty]
        private Thickness panto1Margin = new Thickness(525, 44, 0, 0);
        [ObservableProperty]
        private Thickness panto2Margin = new Thickness(225, 44, 0, 0);

        private readonly double panto1BaseY = 44;
        private readonly double panto2BaseY = 44;

        // Headlights picture 
        [ObservableProperty]
        private Bitmap headlightsImage = LoadAsset("Headlights.png");
        [ObservableProperty]
        private double headlightsOpacity = 0.0; // start off

        public void UpdatePantographImages(bool panto1, bool panto2)
        {
            // Update images
            Panto1Image = panto1 ? panto1Up : panto1Down;
            Panto2Image = panto2 ? panto2Up : panto2Down;

            // Shift DOWN 20px when up
            Panto1Margin = new Thickness(525, panto1 ? panto1BaseY - 13 : panto1BaseY, 0, 0);
            Panto2Margin = new Thickness(225, panto2 ? panto2BaseY - 13 : panto2BaseY, 0, 0);
        }

        public async Task UpdateHeadlightsImage(bool lights)
        {
            // Do nothing if:
            // - state didn't change
            // - animation already running
            if (lights == _headlightsState || _isHeadlightsAnimating)
                return;

            _headlightsState = lights;
            _isHeadlightsAnimating = true;

            double start = HeadlightsOpacity;
            double target = lights ? 1.0 : 0.0;

            const int durationMs = 300;
            const int steps = 30;
            int delay = durationMs / steps;

            for (int i = 1; i <= steps; i++)
            {
                double progress = (double)i / steps;
                HeadlightsOpacity = start + (target - start) * progress;
                await Task.Delay(delay);
            }

            HeadlightsOpacity = target; // ensure exact final value
            _isHeadlightsAnimating = false;
        }
    }
}