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
            Panto1IconImage = pantoIconOff;
            Panto2IconImage = pantoIconOff;
            HeadlightsIconImage = HeadlightsIconOff;

            DirectionForwardImage = directionForwardOff;
            DirectionNeutralImage = directionNeutralOff;
            DirectionBackwardsImage = directionBackwardsOff;

            TrainBrakeIcon1Image = trainBrakeOffImage;
            TrainBrakeIcon2Image = trainBrakeOffImage;
            TrainBrakeIcon3Image = trainBrakeOffImage;
            TrainBrakeIcon4Image = trainBrakeOffImage;

            ParallaxBgImage1 = parallaxImageMountain;
            ParallaxBgImage2 = parallaxImageMountainF;
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

        // panto icons
        private readonly Bitmap pantoIconOn = LoadAsset("ElectricalOn.png");
        private readonly Bitmap pantoIconOff = LoadAsset("ElectricalOff.png");

        [ObservableProperty]
        private Bitmap panto1IconImage;

        [ObservableProperty]
        private Bitmap panto2IconImage;

        // Headlights picture 
        [ObservableProperty]
        private Bitmap headlightsImage = LoadAsset("Headlights.png");
        [ObservableProperty]
        private double headlightsOpacity = 0.0; // start off
        // headlights icon

        private readonly Bitmap HeadlightsIconOn = LoadAsset("HeadlightsIconOn.png");
        private readonly Bitmap HeadlightsIconOff = LoadAsset("HeadlightsIconOff.png");

        [ObservableProperty]
        private Bitmap headlightsIconImage;

        // Direction icons
        private readonly Bitmap directionForwardOn = LoadAsset("EmptyArrowUpOn.png");
        private readonly Bitmap directionForwardOff = LoadAsset("EmptyArrowUpOff.png");

        private readonly Bitmap directionNeutralOn = LoadAsset("EmptyNeutralOn.png");
        private readonly Bitmap directionNeutralOff = LoadAsset("EmptyNeutralOff.png");

        private readonly Bitmap directionBackwardsOn = LoadAsset("EmptyArrowDownOn.png");
        private readonly Bitmap directionBackwardsOff = LoadAsset("EmptyArrowDownOff.png");

        [ObservableProperty]
        private Bitmap directionForwardImage;

        [ObservableProperty]
        private Bitmap directionNeutralImage;

        [ObservableProperty]
        private Bitmap directionBackwardsImage;

        // Throttle bar
        [ObservableProperty]
        private int throttlePercent = 0;

        // engine break rotation
        [ObservableProperty]
        private double directionAngle = 0.0;

        // train break
        // Brake Level Images
        private readonly Bitmap trainBrake0Image = LoadAsset("TrainBrake0.png"); // Highest brake level
        private readonly Bitmap trainBrake1Image = LoadAsset("TrainBrake1.png"); // Brake level 1
        private readonly Bitmap trainBrake2Image = LoadAsset("TrainBrake2.png"); // Brake level 2
        private readonly Bitmap trainBrakeOffImage = LoadAsset("TrainBrakeOff.png"); // Off state

        // Observable properties for each of the brake images
        [ObservableProperty]
        private Bitmap trainBrakeIcon1Image;

        [ObservableProperty]
        private Bitmap trainBrakeIcon2Image;

        [ObservableProperty]
        private Bitmap trainBrakeIcon3Image;

        [ObservableProperty]
        private Bitmap trainBrakeIcon4Image;

        // Parallax animation
        [ObservableProperty]
        private double railsX1 = 0;
        [ObservableProperty]
        private double railsX2 = 800;
        [ObservableProperty]
        private double mountainX1 = 0;
        [ObservableProperty]
        private double mountainX2 = 800;

        private readonly Bitmap parallaxImageMountain = LoadAsset("ParallaxMountain.png"); // Highest brake level
        private readonly Bitmap parallaxImageMountainF = LoadAsset("ParallaxMountainF.png"); // Brake level 1
        private readonly Bitmap parallaxImageCity = LoadAsset("ParallaxCity.png"); // Brake level 2
        [ObservableProperty]
        private Bitmap parallaxBgImage1;
        [ObservableProperty]
        private Bitmap parallaxBgImage2;

        [ObservableProperty]
        private int parallaxIndex = 0;

        public void UpdatePantographImages(bool panto1, bool panto2)
        {
            // Update images
            Panto1Image = panto1 ? panto1Up : panto1Down;
            Panto2Image = panto2 ? panto2Up : panto2Down;

            Panto1IconImage = panto1 ? pantoIconOn : pantoIconOff;
            Panto2IconImage = panto2 ? pantoIconOn : pantoIconOff;

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
            HeadlightsIconImage = lights ? HeadlightsIconOn : HeadlightsIconOff;

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

        private bool _isRailsAnimating = false;
        private double _speed = 0.0;
        public async Task StartRailsAnimation(double speed)
        {
            if (_isRailsAnimating)
                return;

            _isRailsAnimating = true;

            const int delay = 16; // ~60 FPS

            _speed = speed;
            while (_isRailsAnimating)
            {
                double directionSpeed = 0.0;

                if (isDirectionForward())
                    directionSpeed = _speed;
                else if (isDirectionBackwards())
                    directionSpeed = -_speed;

                // Move the elements
                RailsX1 -= directionSpeed;
                RailsX2 -= directionSpeed;
                MountainX1 -= directionSpeed * 0.05; // mountains move slower for parallax
                MountainX2 -= directionSpeed * 0.05;

                // Handle looping for Rails
                if (RailsX1 <= -800)
                    RailsX1 = RailsX2 + 800;
                else if (RailsX1 >= 800)  // Prevent it from going off the other side when moving backwards
                    RailsX1 = RailsX2 - 800;

                if (RailsX2 <= -800)
                    RailsX2 = RailsX1 + 800;
                else if (RailsX2 >= 800)  // Prevent it from going off the other side when moving backwards
                    RailsX2 = RailsX1 - 800;

                // Handle looping for Mountains
                if (MountainX1 <= -800)
                    MountainX1 = MountainX2 + 800;
                else if (MountainX1 >= 800)  // Prevent it from going off the other side when moving backwards
                    MountainX1 = MountainX2 - 800;

                if (MountainX2 <= -800)
                    MountainX2 = MountainX1 + 800;
                else if (MountainX2 >= 800)  // Prevent it from going off the other side when moving backwards
                    MountainX2 = MountainX1 - 800;

                await Task.Delay(delay);
            }
        }
        public void updateSpeed(double speed)
        {
            _speed = speed;
        }

        public void UpdateDirection(int directionPercent)
        {
            // Example logic:
            // < 30%  = Backwards
            // 30–70% = Neutral
            // > 70%  = Forward

            bool isForward = directionPercent > 70;
            bool isNeutral = directionPercent >= 30 && directionPercent <= 70;
            bool isBackwards = directionPercent < 30;

            DirectionForwardImage = isForward ? directionForwardOn : directionForwardOff;
            DirectionNeutralImage = isNeutral ? directionNeutralOn : directionNeutralOff;
            DirectionBackwardsImage = isBackwards ? directionBackwardsOn : directionBackwardsOff;
        }

        private bool isDirectionForward() { return DirectionForwardImage == directionForwardOn; }
        private bool isDirectionNeutral() { return DirectionNeutralImage == directionNeutralOn; }
        private bool isDirectionBackwards() { return DirectionBackwardsImage == directionBackwardsOn; }

        public void StopRailsAnimation()
        {
            _isRailsAnimating = false;
        }

        public void updateThrottle(int throttlePercent)
        {
            ThrottlePercent = throttlePercent;
        }

        public void UpdateEngineBreak(double directionPercent)
        {
            // Rotate based on direction (arbitrary logic for example)
            DirectionAngle = -(directionPercent * 90 / 100);  // Converts the percentage to a full rotation (0-360 degrees)
        }

        public void UpdateTrainBrake(int brakeLevel)
        {
            // Set all to off by default
            TrainBrakeIcon1Image = trainBrakeOffImage;
            TrainBrakeIcon2Image = trainBrakeOffImage;
            TrainBrakeIcon3Image = trainBrakeOffImage;
            TrainBrakeIcon4Image = trainBrakeOffImage;

            switch (brakeLevel)
            {
                case 0:
                    // Highest brake level
                    TrainBrakeIcon4Image = trainBrake0Image;  // Show the highest brake icon
                    break;

                case 1:
                    // Brake Level 1
                    TrainBrakeIcon1Image = trainBrake1Image;  // Show level 1 brake
                    break;

                case 2:
                    // Brake Level 2
                    TrainBrakeIcon2Image = trainBrake2Image;  // Show level 2 brake
                    break;

                case 3:
                    // Brake Level 3
                    TrainBrakeIcon3Image = trainBrake2Image;  // Show level 3 brake (same image as 2, or different image if needed)
                    break;

                default:
                    // Default case, all images should be off
                    TrainBrakeIcon1Image = trainBrakeOffImage;
                    TrainBrakeIcon2Image = trainBrakeOffImage;
                    TrainBrakeIcon3Image = trainBrakeOffImage;
                    TrainBrakeIcon4Image = trainBrakeOffImage;
                    break;
            }
        }

        public void UpdateParallaxBackground()
        {
            if (ParallaxIndex == 0)
            {
                ParallaxBgImage1 = parallaxImageMountain;
                ParallaxBgImage2 = parallaxImageMountainF;
            }
            else if (ParallaxIndex == 1)
            {
                ParallaxBgImage1 = parallaxImageCity;
                ParallaxBgImage2 = parallaxImageCity;
            }
        }
    }
}