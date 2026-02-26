using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using OpenRails_HID_Tester.ViewModels;
using Tmds.DBus.Protocol;
using System;
using HidSharp;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenRails_HID_Tester.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;

        private HidSharp.Device HIDDevice;
        private CancellationTokenSource? _pollingCts;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ConnectHIDDevice(object sender, RoutedEventArgs args)
        {
            if (vm is null && DataContext is MainWindowViewModel _vm)
            {
                vm = _vm;
            }
            if (vm is null)
            {
                return;
            }

            // Initialize HID device list
            var deviceList = DeviceList.Local;

            // Set your device's Vendor ID and Product ID
            int targetVendorId = 0xCAFE;
            int targetProductId = 0x4000;

            HIDDevice = deviceList.GetHidDevices(targetVendorId, targetProductId).FirstOrDefault();

            if (HIDDevice is null)
            {
                vm.DeviceStatus = "Device not found";
                vm.DeviceStatusBrush = Brushes.Red;
                return;
            }
            else
            {
                vm.DeviceStatus = "Connected";
                vm.DeviceStatusBrush = Brushes.Green;
            }

            // Cancel previous polling if any
            _pollingCts?.Cancel();
            _pollingCts = new CancellationTokenSource();

            // Start polling
            StartDevicePolling(_pollingCts.Token);
            /*
            vm.Panto1Status = "Up";
            vm.Panto1StatusBrush = Brushes.Green;
            vm.Panto2Status = "Down";
            vm.Panto2StatusBrush = Brushes.Red;*/
        }
        static int PercentageRangeTrim(int value, int min, int max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return (value - min) * 100 / (max - min);
        }

        private void StartDevicePolling(CancellationToken token)
        {
            Task.Run(async () =>
            {
                _ = vm.StartRailsAnimation(0.0);
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        if (!HIDDevice.TryOpen(out HidSharp.DeviceStream stream))
                        {
                            Console.WriteLine("Failed to open device.");
                            return;
                        }

                        using (stream)
                        {
                            stream.ReadTimeout = 5000;

                            int reportLength = 9; // report ID + 8 data bytes
                            byte[] inputReport = new byte[reportLength];

                            while (true)
                            {
                                int bytesRead = stream.Read(inputReport, 0, reportLength);
                                if (bytesRead == reportLength)
                                {
                                    // Account for report ID at inputReport[0]
                                    int throttle = inputReport[1] | (inputReport[2] << 8);
                                    int direction = inputReport[3] | (inputReport[4] << 8);
                                    int engineBrake = inputReport[5] | (inputReport[6] << 8);

                                    // Convert to percentage
                                    int throttlePercent = PercentageRangeTrim(throttle, 2400, 4090);
                                    int directionPercent = PercentageRangeTrim(direction, 0, 1600);
                                    int engineBrakePercent = PercentageRangeTrim(engineBrake, 0, 4090);

                                    // Digital byte 1: buttons and switches
                                    byte digitalByte1 = inputReport[7];
                                    bool pause = (digitalByte1 & 0x01) != 0;
                                    bool trackMonitor = (digitalByte1 & (1 << 1)) != 0;
                                    bool nextStation = (digitalByte1 & (1 << 2)) != 0;
                                    bool headlights = (digitalByte1 & (1 << 3)) != 0;
                                    bool panto1 = (digitalByte1 & (1 << 4)) != 0;
                                    bool panto2 = (digitalByte1 & (1 << 5)) != 0;

                                    // Digital byte 2: multi-state switches
                                    byte digitalByte2 = inputReport[8];
                                    int view = digitalByte2 & 0x03;
                                    int trainBrake = (digitalByte2 >> 2) & 0x03;

                                    // Update ViewModel using ternary operators
                                    vm.Panto1Status = panto1 ? "Up" : "Down";
                                    vm.Panto1StatusBrush = panto1 ? Brushes.Green : Brushes.Red;

                                    vm.Panto2Status = panto2 ? "Up" : "Down";
                                    vm.Panto2StatusBrush = panto2 ? Brushes.Green : Brushes.Red;

                                    vm.HeadlightsStatus = headlights ? "On" : "Off";
                                    vm.HeadlightsStatusBrush = headlights ? Brushes.Green : Brushes.Red;

                                    vm.PauseStatus = pause ? "Pressed" : "Released";
                                    vm.PauseStatusBrush = pause ? Brushes.Green : Brushes.Red;

                                    vm.StationMonitorStatus = nextStation ? "On" : "Off";
                                    vm.StationMonitorStatusBrush = nextStation ? Brushes.Green : Brushes.Red;

                                    vm.TrackMonitorStatus = trackMonitor ? "On" : "Off";
                                    vm.TrackMonitorStatusBrush = trackMonitor ? Brushes.Green : Brushes.Red;

                                    vm.ViewStatus = view switch
                                    {
                                        0 => "0 - In Cabin view",
                                        1 => "1 - Outside view",
                                        2 => "2 - Cinematic view",
                                        _ => "Unknown"
                                    };

                                    vm.TrainBrakeStatus = trainBrake switch
                                    {
                                        0 => "0 - Emergency",
                                        1 => "1 - Released",
                                        2 => "2 - Applied",
                                        3 => "3 - Applied",
                                        _ => "Unknown"
                                    };
                                    vm.TrainBrakeStatusBrush = trainBrake == 0 ? Brushes.Red : Brushes.Green;

                                    // Percentage values
                                    vm.ThrottleStatus = $"{throttlePercent}%";
                                    vm.ThrottleStatusBrush = throttlePercent > 0 ? Brushes.Green : Brushes.Red;

                                    vm.DirectionStatus = $"{directionPercent}%";
                                    vm.DirectionStatusBrush = directionPercent != 0 ? Brushes.Green : Brushes.Red;

                                    vm.EngineBrakeStatus = $"{engineBrakePercent}%";
                                    vm.EngineBrakeStatusBrush = engineBrakePercent != 0 ? Brushes.Green : Brushes.Red;

                                    // Update images
                                    vm.UpdatePantographImages(panto1, panto2);
                                    // headlights
                                    _ = vm.UpdateHeadlightsImage(headlights);

                                    double speed = throttlePercent / 4.0; // tweak divisor to taste
                                    vm.updateSpeed(speed);

                                    vm.UpdateDirection(directionPercent);
                                    vm.updateThrottle(throttlePercent);
                                    vm.UpdateEngineBreak(engineBrakePercent);
                                    vm.UpdateTrainBrake(trainBrake);
                                }
                            }
                        }


                    }
                    catch (OperationCanceledException)
                    {
                        break; // clean exit
                    }
                    catch (Exception ex)
                    {
                        vm.DeviceStatusBrush = Brushes.Red;
                        vm.DeviceStatus = "Disconnected";
                        _pollingCts?.Cancel();
                    }
                }
                vm.StopRailsAnimation();
            }, token);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vm is null && DataContext is MainWindowViewModel _vm)
            {
                vm = _vm;
            }
            if (vm is null)
            {
                return;
            }

            vm.UpdateParallaxBackground();
        }
    }
}