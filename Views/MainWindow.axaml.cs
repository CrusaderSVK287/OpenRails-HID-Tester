using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using OpenRails_HID_Tester.ViewModels;
using Tmds.DBus.Protocol;

namespace OpenRails_HID_Tester.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ConnectHIDDevice(object sender, RoutedEventArgs args)
        {
            if (vm is null && DataContext is MainWindowViewModel _vm)
            {
                vm = _vm;
            } else
            {
                return;
            }

            /*vm.DeviceStatus = "Connected";
            vm.DeviceStatusBrush = Brushes.Green;

            vm.Panto1Status = "Up";
            vm.Panto1StatusBrush = Brushes.Green;
            vm.Panto2Status = "Down";
            vm.Panto2StatusBrush = Brushes.Red;*/
        }
    }
}