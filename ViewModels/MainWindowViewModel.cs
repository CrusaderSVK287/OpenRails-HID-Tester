using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenRails_HID_Tester.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string deviceStatus = "Unknown";

        [ObservableProperty]
        private string panto1Status = "";

        [ObservableProperty]
        private string panto2Status = "";

        [ObservableProperty]
        private IBrush deviceStatusBrush = Brushes.Red;

        [ObservableProperty]
        private IBrush panto1StatusBrush = Brushes.Black;

        [ObservableProperty]
        private IBrush panto2StatusBrush = Brushes.Black;
    }
}