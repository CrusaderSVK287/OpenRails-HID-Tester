using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenRails_HID_Tester.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string DeviceStatusText { get; } = "Device status: ";

        private string _deviceStatus = "Unknown";
        public string DeviceStatus
        {
            get => _deviceStatus;
            set => SetProperty(ref _deviceStatus, value);
        }

        [ObservableProperty]
        private IBrush deviceStatusBrush = Brushes.Red;
    }
}