using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using OpenRails_HID_Tester.ViewModels;
using Tmds.DBus.Protocol;
using System;
using HidSharp;
using System.Linq;

namespace OpenRails_HID_Tester.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;

        private HidSharp.Device HIDDevice;

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
                
            }
            else
            {
                vm.DeviceStatus = "Connected";
                vm.DeviceStatusBrush = Brushes.Green;
            }

            return;

            /*vm.DeviceStatus = "Connected";
            vm.DeviceStatusBrush = Brushes.Green;

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
    }
}