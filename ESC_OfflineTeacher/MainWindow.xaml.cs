using System.Windows;
using ESC_OfflineTeacher.ViewModel;
using MahApps.Metro.Controls;

namespace ESC_OfflineTeacher
{
    
    public partial class MainWindow : MetroWindow
    {
     
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();


        }


        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DisconnectFlyout.IsOpen)            
                DisconnectFlyout.IsOpen = false;            
            SettingsFlyout.IsOpen=(!SettingsFlyout.IsOpen);

        }

        private void UserButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SettingsFlyout.IsOpen)
                SettingsFlyout.IsOpen = false;
            DisconnectFlyout.IsOpen = (!DisconnectFlyout.IsOpen);
        }
    }
}